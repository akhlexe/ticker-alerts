using Microsoft.AspNetCore.SignalR;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Application.Interfaces.NotificationService.Dtos;

namespace TickerAlert.Infrastructure.NotificationService;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<TickerbloomHub> _hubContext;

    public SignalRNotificationService(IHubContext<TickerbloomHub> hubContext) => _hubContext = hubContext;

    public async Task NotifyAlertTriggered(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }

    public async Task BroadcastAssetPriceUpdate(AssetPriceUpdateDto assetPriceUpdate)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveAssetPriceUpdate", assetPriceUpdate);
    }
}