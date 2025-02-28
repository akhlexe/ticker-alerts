using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TickerAlert.Application.Common.EventBus;
using TickerAlert.Domain.Events;
using TickerAlert.Infrastructure.EventBus.Definitions;
using TickerAlert.Infrastructure.EventBus.Extensions;
using TickerAlert.Infrastructure.EventBus.Settings;

namespace TickerAlert.Infrastructure.EventBus.Service;

internal sealed class RabbitMqEventBus : IEventBus, IDisposable
{
    private readonly IChannel _channel;
    private readonly IReadOnlyDictionary<Type, RabbitMqBindingDefinition> _bindings;
    private readonly Dictionary<Type, List<object>> _consumers = new();
    private readonly ILogger<RabbitMqEventBus> _logger;

    public RabbitMqEventBus(IOptions<RabbitMqSettings> settingsOpts, ILogger<RabbitMqEventBus> logger)
    {
        RabbitMqSettings settings = settingsOpts.Value;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = settings.Host,
            UserName = settings.User,
            Password = settings.Password
        };

        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();


        var bindingDefinitions = RabbitMqDefinitions.GetBindings();

        _bindings = bindingDefinitions.ToDictionary(
            b => GetMessageTypeByRoutingKey(b.RoutingKey),
            b => b
        );

        _channel.InitializeStructure(bindingDefinitions).Wait();
    }

    public async Task SubscribeAsync<T>(IEventConsumer<T> consumer)
    {
        if (!_consumers.ContainsKey(typeof(T)))
        {
            _consumers[typeof(T)] = new List<object>();
        }
        _consumers[typeof(T)].Add(consumer);

        await StartConsumingAsync<T>(); // Start consuming messages for this type
    }

    private async Task StartConsumingAsync<T>()
    {
        if (!_consumers.ContainsKey(typeof(T))) return; // No consumers for this type

        var binding = ResolveBindingForMessageType<T>();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(body);

            if (message != null)
            {
                foreach (var registeredConsumer in _consumers[typeof(T)].Cast<IEventConsumer<T>>())
                {
                    try
                    {
                        await registeredConsumer.HandleAsync(message, default);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error handling message");
                    }
                }
                // Acknowledge message
                await _channel.BasicAckAsync(ea.DeliveryTag, false); 
            }
            else
            {
                _logger.LogError("Error deserializing message");
                // Negative acknowledge to requeue message.
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }

        };

        await _channel.BasicConsumeAsync(queue: binding.Queue, autoAck: false, consumer: consumer); // autoAck: false is important
    }

    public ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var binding = ResolveBindingForMessageType<T>();
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _logger.LogInformation("Publishing message to {Exchange} with RoutingKey {RoutingKey}", binding.Exchange, binding.RoutingKey);

        return _channel.BasicPublishAsync(
            exchange: binding.Exchange,
            routingKey: binding.Queue,
            false,
            basicProperties: new BasicProperties(),
            body: body,
            cancellationToken: cancellationToken);
    }

    private RabbitMqBindingDefinition ResolveBindingForMessageType<T>()
    {
        if (!_bindings.TryGetValue(typeof(T), out var binding))
        {
            throw new InvalidOperationException($"No RabbitMQ binding found for message type {typeof(T).Name}");
        }

        return binding;
    }

    private static Type GetMessageTypeByRoutingKey(string routingKey) =>
       routingKey switch
       {
           "price.readed" => typeof(PriceUpdateEvent),
           "alert.triggered" => typeof(AlertTriggeredEvent),
           _ => throw new InvalidOperationException($"Unknown routing key: {routingKey}")
       };

    public void Dispose() => _channel?.Dispose();
}
