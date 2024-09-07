using Microsoft.AspNetCore.SignalR;

namespace TickerAlert.Infrastructure.NotificationService;

public class TickerbloomHub : Hub 
{
    private static Dictionary<string, HashSet<string>> userStockSubscriptions = new();

    public Task SubscribeToStock(string stockSymbol)
    {
        var userId = Context.ConnectionId;

        if (!userStockSubscriptions.ContainsKey(userId))
        {
            userStockSubscriptions[userId] = new HashSet<string>();
        }
        userStockSubscriptions[userId].Add(stockSymbol);

        return Task.CompletedTask;
    }
}