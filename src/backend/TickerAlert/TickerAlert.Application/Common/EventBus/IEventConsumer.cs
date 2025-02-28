namespace TickerAlert.Application.Common.EventBus;

public interface IEventConsumer<T>
{
    Task HandleAsync(T eventMessage, CancellationToken cancellationToken);
}
