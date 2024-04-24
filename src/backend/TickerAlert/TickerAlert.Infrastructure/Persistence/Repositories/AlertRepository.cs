using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly ApplicationDbContext _context;
    public AlertRepository(ApplicationDbContext context) => _context = context;

    public async Task CreateAlert(int userId, int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType)
    {
        var alert = new Alert(
            userId,
            financialAssetId, 
            targetPrice,
            thresholdType
        );
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Alert>> GetAllForUserId(int userId)
    {
        return await _context.Alerts
            .Include(a => a.FinancialAsset)
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}