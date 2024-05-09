using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface IPriceMeasureRepository
{
    Task<PriceMeasure?> GetLastPriceMeasureFor(Guid financialAssetId);
    Task<List<PriceMeasure?>> GetLastPricesMeasuresFor(IEnumerable<Guid> financialAssetsIds);
    Task RegisterPriceMeasure(PriceMeasure measure);
    Task<PriceMeasure?> GetById(Guid id);
}