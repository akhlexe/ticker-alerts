using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

public class PriceMeasureReader(IApplicationDbContext context) : IPriceMeasureReader
{
    public async Task<PriceMeasure?> GetLastPriceMeasureFor(Guid financialAssetId) 
        => await context.PriceMeasures
            .AsNoTracking()
            .Where(p => p.FinancialAssetId == financialAssetId)
            .OrderByDescending(p => p.MeasuredOn)
            .FirstOrDefaultAsync();

    public async Task<List<PriceMeasure?>> GetLastPricesMeasuresFor(IEnumerable<Guid> financialAssetsIds) 
        => await context.PriceMeasures
            .AsNoTracking()
            .Where(x => financialAssetsIds.Contains(x.FinancialAssetId))
            .GroupBy(x => x.FinancialAssetId)
            .Select(g => g.OrderByDescending(r => r.MeasuredOn).FirstOrDefault())
            .ToListAsync();

    public async Task<PriceMeasure?> GetById(Guid id) 
        => await context
            .PriceMeasures
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
}