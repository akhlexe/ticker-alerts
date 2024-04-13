using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly ApplicationDbContext _context;

    public AlertRepository(ApplicationDbContext context) => _context = context;

    public async Task CreateAlert(int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType)
    {
        var alert = new Alert(
            financialAssetId, 
            targetPrice,
            thresholdType
        );
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Alert>> GetAll()
    {
        return await _context.Alerts
            .Include(a => a.FinancialAsset)
            .AsNoTracking()
            .ToListAsync();
    }
}