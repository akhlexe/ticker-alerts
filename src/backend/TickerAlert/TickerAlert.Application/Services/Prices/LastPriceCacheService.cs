using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.PriceMeasures;

namespace TickerAlert.Application.Services.Prices;

public class LastPriceCacheService(ICacheService cacheService) : ILastPriceCacheService
{
    private const string NamespacePrefix = "LastPrice";

    public async Task<decimal?> GetPriceAsync(Guid financialAssetId) 
        => await cacheService.GetAsync<decimal>(NamespacePrefix, financialAssetId.ToString());

    public async Task<Dictionary<Guid, decimal>> GetPricesAsync(IEnumerable<Guid> financialAssetIds)
    {
        var fetchedPrices = await cacheService.GetMultipleAsync<decimal>(
            NamespacePrefix, 
            financialAssetIds.Select(a => a.ToString())
        );

        return fetchedPrices.ToDictionary(
            pair => Guid.Parse(pair.Key),
            pair => pair.Value
        );
    }

    public async Task UpdatePriceAsync(Guid financialAssetId, decimal price) 
        => await cacheService.SetAsync(NamespacePrefix, financialAssetId.ToString(), price, TimeSpan.FromMinutes(10));
}
