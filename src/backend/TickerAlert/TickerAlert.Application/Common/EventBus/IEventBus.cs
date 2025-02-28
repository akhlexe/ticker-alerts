namespace TickerAlert.Application.Common.EventBus;

public interface IEventBus
{
    ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}
