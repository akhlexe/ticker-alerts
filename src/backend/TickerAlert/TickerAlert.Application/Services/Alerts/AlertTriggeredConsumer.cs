using Microsoft.Extensions.Logging;
using TickerAlert.Application.Common.EventBus;
using TickerAlert.Application.Services.Notifiers;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Alerts;

public class AlertTriggeredConsumer(
    AlertTriggeredNotifier alertTriggeredNotifier,
    ILogger<AlertTriggeredConsumer> logger) : IEventConsumer<AlertTriggeredEvent>
{
    public async Task HandleAsync(AlertTriggeredEvent eventMessage, CancellationToken cancellationToken)
    {
        await alertTriggeredNotifier.Notify(eventMessage.AlertId);
        logger.LogInformation("Alert triggered (Id = {AlertId})", eventMessage.AlertId);
    }
}