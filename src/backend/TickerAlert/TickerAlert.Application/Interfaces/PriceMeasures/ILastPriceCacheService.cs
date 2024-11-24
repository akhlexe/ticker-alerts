namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface ILastPriceCacheService
{
    Task<decimal?> GetPrice(Guid financialAssetId);
    Task<Dictionary<Guid, decimal>> GetPrices(IEnumerable<Guid> financialAssetIds);
    Task UpdatePrice(Guid financialAssetId, decimal price);
}
