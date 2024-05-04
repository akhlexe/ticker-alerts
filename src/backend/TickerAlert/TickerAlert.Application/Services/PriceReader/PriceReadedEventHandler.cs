using MediatR;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.PriceReader;

public class PriceReadedEventHandler 
    : INotificationHandler<PriceReadedDomainEvent>
{
    public Task Handle(PriceReadedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}