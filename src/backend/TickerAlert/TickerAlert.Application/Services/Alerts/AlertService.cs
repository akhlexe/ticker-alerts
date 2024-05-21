using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Events;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.Alerts;

public class AlertService : IAlertService
{
    private readonly IApplicationDbContext _context;
    private readonly IPriceMeasureReader _priceMeasureReader;
    private readonly ICurrentUserService _currentUserService;

    public AlertService(
        IApplicationDbContext context,
        IPriceMeasureReader priceMeasureReader, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _priceMeasureReader = priceMeasureReader;
        _currentUserService = currentUserService;
    }

    public async Task CreateAlert(Guid financialAssetId, decimal targetPrice)
    {
        var threshold = await ResolveThreshold(financialAssetId, targetPrice);
        var newAlert = Alert.Create(Guid.NewGuid(), _currentUserService.UserId, financialAssetId, targetPrice, threshold);

        _context.Alerts.Add(newAlert);
        await _context.SaveChangesAsync();
    }

    public async Task TriggerAlert(Alert alert)
    {
        alert.State = AlertState.TRIGGERED;
        alert.RaiseDomainEvent(new AlertTriggeredDomainEvent(Guid.NewGuid(), alert.Id));
        
        _context.Alerts.Update(alert);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyAlert(Alert alert)
    {
        alert.State = AlertState.NOTIFIED;
        
        _context.Alerts.Update(alert);
        
        // TODO: notify alert with domain event.
        await _context.SaveChangesAsync();
    }
    
    private async Task<PriceThresholdType> ResolveThreshold(Guid financialAssetId, decimal targetPrice)
    {
        decimal currentPrice = await GetCurrentPrice(financialAssetId);
        return ThresholdResolver.Resolve(currentPrice, targetPrice);
    }

    private async Task<decimal> GetCurrentPrice(Guid financialAssetId)
    {
        var priceMeasure = await _priceMeasureReader.GetLastPriceMeasureFor(financialAssetId);
        return priceMeasure?.Price ?? 0;
    }
}