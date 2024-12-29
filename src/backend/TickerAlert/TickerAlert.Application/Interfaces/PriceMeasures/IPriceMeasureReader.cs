using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface IPriceMeasureReader
{
    Task<decimal> GetLastPriceFor(Guid financialAssetId);
    Task<Dictionary<Guid, decimal>> GetLastPricesFor(IEnumerable<Guid> financialAssetsIds);
    Task<PriceMeasure?> GetById(Guid id);
}