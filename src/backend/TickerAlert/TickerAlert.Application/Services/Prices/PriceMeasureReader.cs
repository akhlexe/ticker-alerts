using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

public class PriceMeasureReader : IPriceMeasureReader
{
    private readonly IApplicationDbContext _context;

    public PriceMeasureReader(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PriceMeasure?> GetLastPriceMeasureFor(Guid financialAssetId)
    {
        return await _context.PriceMeasures
            .AsNoTracking()
            .Where(p => p.FinancialAssetId == financialAssetId)
            .OrderByDescending(p => p.MeasuredOn)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PriceMeasure>> GetLastPricesMeasuresFor(IEnumerable<Guid> financialAssetsIds)
    {
        return await _context.PriceMeasures
            .AsNoTracking()
            .Where(x => financialAssetsIds.Contains(x.FinancialAssetId))
            .GroupBy(x => x.FinancialAssetId)
            .Select(g => g.OrderByDescending(r => r.MeasuredOn).First())
            .ToListAsync();
    }
    
    public async Task<PriceMeasure?> GetById(Guid id)
    {
        return await _context
            .PriceMeasures
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PriceMeasure>> GetYesterdayClosePricesFor(IEnumerable<Guid> financialAssetsIds)
    {
        var yesterdayStart = DateTime.UtcNow.Date.AddDays(-1);
        var yesterdayEnd = yesterdayStart.AddDays(1);

        return await _context
            .PriceMeasures
            .AsNoTracking()
            .Where(x => financialAssetsIds.Contains(x.FinancialAssetId) &&
                    x.MeasuredOn >= yesterdayStart &&
                    x.MeasuredOn < yesterdayEnd)
            .GroupBy(x => x.FinancialAssetId)
            .Select(g => g.OrderByDescending(r => r.MeasuredOn).First())
            .ToListAsync();
    }
}