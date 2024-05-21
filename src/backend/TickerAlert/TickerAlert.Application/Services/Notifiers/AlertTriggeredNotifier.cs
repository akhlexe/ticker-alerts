using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Notifiers;

public class AlertTriggeredNotifier
{
    private readonly IAlertReader _alertReader;
    private readonly IAlertService _alertService;
    private readonly INotificationService _notificationService;

    public AlertTriggeredNotifier(
        INotificationService notificationService, 
        IAlertReader alertReader, 
        IAlertService alertService)
    {
        _notificationService = notificationService;
        _alertReader = alertReader;
        _alertService = alertService;
    }

    public async Task Notify(Guid alertId)
    {
        try
        {
            var alertTriggered = await _alertReader.GetById(alertId);
        
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
        await _alertService.NotifyAlert(alert);
    }

    private async Task SendNotification(Alert alert)
    {
        var message = CreateMessage(alert);
        
        await _notificationService.Notify(
            alert.UserId.ToString(),
            message
        );
    }

    private static string CreateMessage(Alert alert)
    {
        return alert.PriceThreshold switch
        {
            PriceThresholdType.Above => PriceCrossingAboveTargetMessage(alert),
            PriceThresholdType.Below => PriceCrossingBelowTargetMessage(alert),
            _ => throw new NotSupportedException()
        };
    }

    private static string PriceCrossingBelowTargetMessage(Alert alert) => 
        $"{alert.FinancialAsset.Name} ({alert.FinancialAsset.Ticker}) acaba pasar debajo del precio {alert.TargetPrice}.";

    private static string PriceCrossingAboveTargetMessage(Alert alert) 
        => $"{alert.FinancialAsset.Name} ({alert.FinancialAsset.Ticker}) acaba de sobrepasar el precio {alert.TargetPrice}.";
}