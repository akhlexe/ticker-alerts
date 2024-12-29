using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

public class PriceMeasureReader(
    IApplicationDbContext context,
    ILastPriceCacheService cacheService) : IPriceMeasureReader
{
    public async Task<decimal> GetLastPriceFor(Guid financialAssetId)
    {
        Dictionary<Guid, decimal> lastPrices = await GetLastPricesFor([financialAssetId]);

        return lastPrices.GetValueOrDefault(financialAssetId);
    }

    public async Task<Dictionary<Guid, decimal>> GetLastPricesFor(IEnumerable<Guid> financialAssetsIds)
    {
        Dictionary<Guid, decimal> lastPrices = await cacheService.GetPricesAsync(financialAssetsIds);
        Dictionary<Guid, decimal> missingPrices = await FetchAndSaveMissingPricesInCache(context, cacheService, financialAssetsIds.Except(lastPrices.Keys).ToList());

        return lastPrices
            .Concat(missingPrices)
            .GroupBy(pair => pair.Key)
            .ToDictionary(group => group.Key, group => group.Last().Value);
    }

    private static async Task<Dictionary<Guid, decimal>> FetchAndSaveMissingPricesInCache(IApplicationDbContext context, ILastPriceCacheService cacheService, IEnumerable<Guid> financialAssetsIds)
    {
        if (!financialAssetsIds.Any()) return [];

        List<PriceMeasure> priceMeasures = await FetchLastPricesFromDatabaseAsync(context, financialAssetsIds);

        foreach (var price in priceMeasures)
        {
            await cacheService.UpdatePriceAsync(price.FinancialAssetId, price.Price);
        }

        return priceMeasures.ToDictionary(p => p.FinancialAssetId, p => p.Price);
    }

    private static async Task<List<PriceMeasure>> FetchLastPricesFromDatabaseAsync(IApplicationDbContext context, IEnumerable<Guid> financialAssetsIds)
    {
        return await context
            .PriceMeasures
            .AsNoTracking()
            .Where(x => financialAssetsIds.Distinct().Contains(x.FinancialAssetId))
            .GroupBy(x => x.FinancialAssetId)
            .Select(g => g.OrderByDescending(r => r.MeasuredOn).First())
            .ToListAsync();
    }

    public async Task<PriceMeasure?> GetById(Guid id)
    {
        return await context
            .PriceMeasures
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}