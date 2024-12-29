using TickerAlert.Application.Interfaces.NotificationService.Dtos;

namespace TickerAlert.Application.Interfaces.NotificationService;

public interface INotificationService
{
    Task NotifyAlertTriggered(string userId, string message);
    Task BroadcastAssetPriceUpdate(AssetPriceUpdateDto assetPriceUpdate);
}