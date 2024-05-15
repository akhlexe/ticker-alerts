using MediatR;
using TickerAlert.Application.Services.Notifiers;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Alerts;

public class AlertTriggeredEventHandler : INotificationHandler<AlertTriggeredDomainEvent>
{
    private readonly AlertTriggeredNotifier _alertTriggeredNotifier;

    public AlertTriggeredEventHandler(AlertTriggeredNotifier alertTriggeredNotifier)
    {
        _alertTriggeredNotifier = alertTriggeredNotifier;
    }

    public async Task Handle(AlertTriggeredDomainEvent notification, CancellationToken cancellationToken)
    {
        await _alertTriggeredNotifier.Notify(notification.AlertId);
    }
}