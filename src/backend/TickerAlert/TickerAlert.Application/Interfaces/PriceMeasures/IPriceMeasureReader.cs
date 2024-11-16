using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface IPriceMeasureReader
{
    Task<PriceMeasure?> GetLastPriceMeasureFor(Guid financialAssetId);
    Task<List<PriceMeasure>> GetLastPricesMeasuresFor(IEnumerable<Guid> financialAssetsIds);
    Task<List<PriceMeasure>> GetYesterdayClosePricesFor(IEnumerable<Guid> financialAssetsIds);
    Task<PriceMeasure?> GetById(Guid id);
}