using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

public sealed record PriceReaded(Guid FinancialAssetId, decimal Value);

public sealed class PriceReadedNotifier(
    INotificationService notificationService, 
    IPriceMeasureReader priceMeasureReader,
    IWatchlistStockSubscriptionService watchlistSubscriptions)
{
    public async Task NotifyPriceReaded(Guid priceMeasureId)
    {
        PriceMeasure? priceMeasure = await priceMeasureReader.GetById(priceMeasureId);

        HashSet<string> userIdsToNotify = await watchlistSubscriptions
            .GetUsersSubscribedToStock(priceMeasure?.FinancialAssetId ?? Guid.Empty);

        foreach (string userId in userIdsToNotify)
        {
            await notificationService.Notify(
                userId,
                $"Timestamp: {DateTime.UtcNow}, AssetId = {priceMeasure?.FinancialAssetId}, new Price = ${priceMeasure?.Price}.");
        }
    }
}
