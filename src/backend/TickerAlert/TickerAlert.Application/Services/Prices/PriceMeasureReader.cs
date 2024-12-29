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
        decimal? lastPrice = await cacheService.GetPriceAsync(financialAssetId);

        // TODO: si no existe el precio en cache, buscarlo y salvarlo.
        // Pero se debería extraer la lógica en una funcionalidad unica, pero capaz haya que pensarla
        // un poco más para no duplicar el código o dejar las cosas medias rebuscadas.

        return lastPrice ?? 0;
    }

    public async Task<Dictionary<Guid, decimal>> GetLastPricesFor(IEnumerable<Guid> financialAssetsIds)
    {
        Dictionary<Guid, decimal> lastPrices = await cacheService.GetPricesAsync(financialAssetsIds);

        List<Guid> missingIds = financialAssetsIds.Except(lastPrices.Keys).ToList();

        if (missingIds.Any())
        {
            List<PriceMeasure> priceMeasures = await FetchLastPricesFromDatabaseAsync(context, missingIds);

            foreach (var price in priceMeasures)
            {
                await cacheService.UpdatePriceAsync(price.FinancialAssetId, price.Price);
                lastPrices[price.FinancialAssetId] = price.Price;
            }
        }

        return lastPrices;
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