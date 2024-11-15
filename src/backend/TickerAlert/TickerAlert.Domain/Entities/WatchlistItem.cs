using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class WatchlistItem : Entity
{
    public Guid FinancialAssetId { get; private set; }

    public static WatchlistItem Create(Guid id, Guid financialAssetId) => new(id, financialAssetId);

    private WatchlistItem(Guid id, Guid financialAssetId) : base(id)
    {
        FinancialAssetId = id;
    }
}
