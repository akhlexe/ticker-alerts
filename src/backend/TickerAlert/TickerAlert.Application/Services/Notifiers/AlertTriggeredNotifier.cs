using TickerAlert.Application.Common.Mailing;
using TickerAlert.Application.Common.Mailing.Dtos;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Notifiers;

public class AlertTriggeredNotifier(
    INotificationService notificationService,
    IAlertReader alertReader,
    IAlertService alertService,
    IEmailService emailService)
{
    public async Task Notify(Guid alertId)
    {
        try
        {
            var alertTriggered = await alertReader.GetById(alertId);
            if (alertTriggered == null) return;
            
            await SendNotification(alertTriggered);
            await SendAlertTriggeredEmail(alertTriggered);
            await UpdateAlertState(alertTriggered);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task SendNotification(Alert alert)
        => await notificationService.Notify(alert.UserId.ToString(), CreateMessage(alert));

    private async Task SendAlertTriggeredEmail(Alert alertTriggered)
    {
        var email = new Email(alertTriggered.User.Username, "Alert Triggered");
        var data = new AlertTriggeredData(
            alertTriggered.FinancialAsset.Name, 
            alertTriggered.FinancialAsset.Ticker,
            alertTriggered.TargetPrice,
            CreateMessage(alertTriggered), 
            DateTime.UtcNow.ToString());

        await emailService.SendAlertTriggeredEmail(email, data);
    }

    private async Task UpdateAlertState(Alert alert)
    {
        await alertService.NotifyAlert(alert);
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
        $"{alert.FinancialAsset.Name} ({alert.FinancialAsset.Ticker}) has just crossed below the target price of {alert.TargetPrice}.";

    private static string PriceCrossingAboveTargetMessage(Alert alert) 
        => $"{alert.FinancialAsset.Name} ({alert.FinancialAsset.Ticker}) as just crossed above the target price of {alert.TargetPrice}.";
}