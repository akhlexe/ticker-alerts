using Microsoft.AspNetCore.SignalR;
using TickerAlert.Application.Services.StockMarket;

namespace TickerAlert.Infrastructure.NotificationService;

public class TickerbloomHub(IStockSubscriptionService stockSubscriptionService) : Hub 
{
    public async Task SubscribeToStock(string stockSymbol)
    {
        var userId = Context.ConnectionId;
        await stockSubscriptionService.AddSubscription(userId, stockSymbol);
    }

    public async Task RemoveSubscription(string userId, string stockSymbol)
    {
        await stockSubscriptionService.RemoveSubscription(userId, stockSymbol);
    }
}