using Microsoft.AspNetCore.SignalR;
using TickerAlert.Application.Interfaces.NotificationService;

namespace TickerAlert.Infrastructure.NotificationService;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<AlertTriggeredHub> _hubContext;

    public SignalRNotificationService(IHubContext<AlertTriggeredHub> hubContext) 
        => _hubContext = hubContext;

    public async Task Notify(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }
}