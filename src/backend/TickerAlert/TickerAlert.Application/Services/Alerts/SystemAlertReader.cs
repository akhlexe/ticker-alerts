using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Alerts;

public class SystemAlertReader(IApplicationDbContext context) : ISystemAlertReader
{
    public async Task<IEnumerable<Alert>> GetPendingAlertsByFinancialAsset(Guid financialAssetId)
    {
        return await context
            .Alerts
            .AsNoTracking()
            .Where(x => x.State == AlertState.PENDING && x.FinancialAssetId == financialAssetId)
            .ToListAsync();
    }
}