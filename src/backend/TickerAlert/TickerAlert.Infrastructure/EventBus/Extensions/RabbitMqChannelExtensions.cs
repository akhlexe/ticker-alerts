using RabbitMQ.Client;
using TickerAlert.Infrastructure.EventBus.Extensions;

namespace TickerAlert.Infrastructure.EventBus.Extensions;

public static class RabbitMqChannelExtensions
{
    public static async Task InitializeStructure(this IChannel channel, IEnumerable<RabbitMqBindingDefinition> bindings)
    {
        var exchangeTasks = bindings.Select(b => b.Exchange).Distinct().Select(channel.CreateExchange).ToList();
        var queueTasks = bindings.Select(b => b.Queue).Distinct().Select(channel.CreateQueue).ToList();

        await Task.WhenAll(exchangeTasks);
        await Task.WhenAll(queueTasks);
        await Task.WhenAll(bindings.Select(channel.BindQueue).ToList());
    }

    public static Task CreateExchange(this IChannel channel, string exchange) 
        => channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct);

    public static Task CreateQueue(this IChannel channel, string queue) 
        => channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);

    public static Task BindQueue(this IChannel channel, RabbitMqBindingDefinition binding) 
        => channel.QueueBindAsync(queue: binding.Queue, exchange: binding.Exchange, routingKey: binding.RoutingKey);
}


public sealed record RabbitMqBindingDefinition(string Exchange, string Queue, string RoutingKey);