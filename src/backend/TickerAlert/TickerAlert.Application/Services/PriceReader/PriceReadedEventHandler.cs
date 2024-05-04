using MediatR;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.PriceReader;

public class PriceReadedEventHandler 
    : INotificationHandler<PriceReadedDomainEvent>
{
    public Task Handle(PriceReadedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Recibo en handler el precio leido nuevo con Id => {notification.PriceMeasureId}.");

        return Task.CompletedTask;
    }
}