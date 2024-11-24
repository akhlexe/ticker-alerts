using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.PriceMeasures;

namespace TickerAlert.Application.Services.Prices;

public class LastPriceCacheService : ILastPriceCacheService
{
    private const string NamespacePrefix = "LastPrice";
    private readonly ICacheService _cacheService;

    public LastPriceCacheService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<decimal?> GetPrice(Guid financialAssetId) 
        => await _cacheService.GetAsync<decimal>(NamespacePrefix, financialAssetId.ToString());

    public async Task<Dictionary<Guid, decimal>> GetPrices(IEnumerable<Guid> financialAssetIds)
    {
        Dictionary<string, decimal> fetchedPrices = await _cacheService.GetMultipleAsync<decimal>(NamespacePrefix, financialAssetIds.Select(a => a.ToString()));

        return fetchedPrices.ToDictionary(
            pair => Guid.Parse(pair.Key),
            pair => pair.Value
        );
    }

    public async Task UpdatePrice(Guid financialAssetId, decimal price) 
        => await _cacheService.SetAsync(NamespacePrefix, financialAssetId.ToString(), price, TimeSpan.FromMinutes(10));
}
