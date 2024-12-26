namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface ILastPriceCacheService
{
    Task<decimal?> GetPriceAsync(Guid financialAssetId);
    Task<Dictionary<Guid, decimal>> GetPricesAsync(IEnumerable<Guid> financialAssetIds);
    Task UpdatePriceAsync(Guid financialAssetId, decimal price);
}
