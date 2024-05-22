using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Alerts;

public class SystemAlertReader : ISystemAlertReader
{
    private readonly IApplicationDbContext _context;

    public SystemAlertReader(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Alert>> GetPendingAlertsByFinancialAsset(Guid financialAssetId)
    {
        return await _context
            .Alerts
            .AsNoTracking()
            .Where(x => x.State == AlertState.PENDING && x.FinancialAssetId == financialAssetId)
            .ToListAsync();
    }
}