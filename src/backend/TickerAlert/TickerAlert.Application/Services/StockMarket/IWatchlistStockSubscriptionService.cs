
namespace TickerAlert.Application.Services.StockMarket;

public interface IWatchlistStockSubscriptionService
{
    Task AddSubscription(Guid assetId, string userId);
    Task<HashSet<string>> GetUsersSubscribedToStock(Guid assetId);
    Task RemoveSubscription(Guid assetId, string userId);
}