using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class FinancialAssetRepository : IFinancialAssetRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialAssetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialAsset>> GetAllBySearchCriteria(string criteria)
    {
        return await _context.FinancialAssets
            .Where(x => x.Name.Contains(criteria) || x.Ticker.Contains(criteria))
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialAsset>> GetAllWithPendingAlerts()
    {
        // IEnumerable<int> pendingAssetsIds = await _context
        //     .Alerts
        //     .Where(x => x.State == AlertState.PENDING)
        //     .Select(x => x.Id)
        //     .Distinct()
        //     .ToListAsync();
        //
        // return await _context
        //     .FinancialAssets
        //     .Where(x => pendingAssetsIds.Contains(x.Id))
        //     .ToListAsync();
        
        return await _context.FinancialAssets
            .Join(_context.Alerts,
                fa => fa.Id,
                alert => alert.FinancialAssetId,
                (fa, alert) => new { FinancialAsset = fa, Alert = alert })
            .Where(faAlert => faAlert.Alert.State == AlertState.PENDING)
            .Select(faAlert => faAlert.FinancialAsset)
            .Distinct()
            .ToListAsync();
    }
}