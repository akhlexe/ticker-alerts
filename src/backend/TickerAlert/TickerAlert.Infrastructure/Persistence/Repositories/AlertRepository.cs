using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Events;
using TickerAlert.Infrastructure.Persistence.Outbox;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly ApplicationDbContext _context;
    public AlertRepository(ApplicationDbContext context) => _context = context;

    public async Task<Alert?> GetById(Guid alertId)
    {
        return await _context.Alerts.FindAsync(alertId);
    }

    public async Task CreateAlert(Guid userId, Guid financialAssetId, decimal targetPrice, PriceThresholdType thresholdType)
    {
        var alert = Alert.Create(Guid.NewGuid(), userId, financialAssetId, targetPrice, thresholdType);
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Alert>> GetAllForUserId(Guid userId)
    {
        return await _context.Alerts
            .Include(a => a.FinancialAsset)
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetAllWithPendingStateAndByFinancialAssetId(Guid financialAssetId)
    {
        return await _context
            .Alerts
            .Where(x => x.FinancialAssetId == financialAssetId && x.State == AlertState.PENDING)
            .ToListAsync();
    }

    // // Ver si queda, sino lo borro.
    // public async Task UpdateRange(IEnumerable<Alert> alerts)
    // {
    //     _context.Alerts.UpdateRange(alerts);
    //     await _context.SaveChangesAsync();
    // }

    public async Task TriggerAlert(Alert alert)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            alert.State = AlertState.TRIGGERED;
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync();

            var domainEvent = new AlertTriggeredDomainEvent(Guid.NewGuid(), alert.Id);
            _context.OutboxMessages.Add(OutboxMessageBuilder.CreateOutboxMessage(domainEvent));
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task NotifyAlert(Alert alert)
    {
        alert.State = AlertState.NOTIFIED;
        await _context.SaveChangesAsync();
    }
}