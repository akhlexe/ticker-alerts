
namespace TickerAlert.Application.Services.StockMarket;

public interface IStockSubscriptionService
{
    Task AddSubscription(string stockSymbol, string userId);
    Task<HashSet<string>> GetUsersSubscribedToStock(string stockSymbol);
    Task RemoveSubscription(string stockSymbol, string userId);
}