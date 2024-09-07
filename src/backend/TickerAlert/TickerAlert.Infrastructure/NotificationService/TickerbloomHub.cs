using Microsoft.AspNetCore.SignalR;
using TickerAlert.Application.Services.StockMarket;

namespace TickerAlert.Infrastructure.NotificationService;

public class TickerbloomHub(IWatchlistStockSubscriptionService stockSubscriptionService) : Hub 
{
    public async Task SubscribeToStock(string stockSymbol)
    {
        var userId = Context.ConnectionId;
        await stockSubscriptionService.AddSubscription(Guid.Parse(userId), stockSymbol);
    }

    public async Task RemoveSubscription(string userId, string stockSymbol)
    {
        await stockSubscriptionService.RemoveSubscription(Guid.Parse(userId), stockSymbol);
    }
}