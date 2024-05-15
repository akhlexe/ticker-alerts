using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Notifiers;

public class AlertTriggeredNotifier
{
    private readonly IAlertRepository _alertRepository;
    private readonly INotificationService _notificationService;

    public AlertTriggeredNotifier(IAlertRepository alertRepository, INotificationService notificationService)
    {
        _alertRepository = alertRepository;
        _notificationService = notificationService;
    }

    public async Task Notify(Guid alertId)
    {
        try
        {
            var alertTriggered = await _alertRepository.GetById(alertId);

            if (alertTriggered == null) return;
            
            await SendNotification(alertTriggered);
            await UpdateAlertState(alertTriggered);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task UpdateAlertState(Alert alert)
    {
        await _alertRepository.NotifyAlert(alert);
    }

    private async Task SendNotification(Alert alert)
    {
        var message = $"Alerta hiteada Id = {alert.Id}, Price = {alert.TargetPrice}";

        await _notificationService.Notify(
            alert.UserId.ToString(),
            message
        );
    }
}