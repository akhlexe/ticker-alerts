using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class WatchlistItem : Entity
{
    public Guid FinancialAssetId { get; private set; }
    public Guid WatchlistId { get; private set; }

    public static WatchlistItem Create(Guid id, Guid watchlistId, Guid financialAssetId) 
        => new(id, watchlistId, financialAssetId);

    private WatchlistItem(Guid id, Guid watchlistId, Guid financialAssetId) : base(id)
    {
        FinancialAssetId = financialAssetId;
        WatchlistId = watchlistId;
    }
}
