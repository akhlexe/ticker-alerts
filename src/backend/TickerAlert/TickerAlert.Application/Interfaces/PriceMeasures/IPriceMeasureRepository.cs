using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface IPriceMeasureRepository
{
    Task<PriceMeasure?> GetLastPriceMeasureFor(int financialAssetId);
    Task<List<PriceMeasure?>> GetLastPricesMeasuresFor(IEnumerable<int> financialAssetsIds);
    Task RegisterPriceMeasure(PriceMeasure measure);
}