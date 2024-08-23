using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Alerts;

public class AlertReader : IAlertReader
{
    private readonly IApplicationDbContext _context;
    private readonly IPriceMeasureReader _priceMeasureReader;
    private readonly ICurrentUserService _currentUserService;
    
    public AlertReader(
        IApplicationDbContext context,
        IPriceMeasureReader priceMeasureReader, 
        ICurrentUserService currentUserService) 
    {
        _context = context;
        _priceMeasureReader = priceMeasureReader;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        var alerts = await GetAllForUserId(_currentUserService.UserId);
        var lastPrices = await _priceMeasureReader.GetLastPricesMeasuresFor(alerts.Select(x => x.FinancialAssetId));
        return alerts.Select(alert => CreateAlertDto(alert, lastPrices.FirstOrDefault(p => p.FinancialAssetId == alert.FinancialAssetId)));
    }

    public async Task<Alert?> GetById(Guid alertId)
    {
        return await _context
            .Alerts
            .Include(x => x.FinancialAsset)
            .FirstOrDefaultAsync(x => x.Id == alertId);
    }

    public async Task<IEnumerable<Alert>> GetAllForUserId(Guid userId)
    {
        return await _context.Alerts
            .Include(a => a.FinancialAsset)
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    private static AlertDto CreateAlertDto(Alert a, PriceMeasure? priceMeasure)
    {
        var actualPrice = priceMeasure?.Price ?? 0;

        return new AlertDto()
        {
            Id = a.Id,
            FinancialAssetId = a.FinancialAssetId,
            TickerName = a.FinancialAsset.Ticker,
            TargetPrice = a.TargetPrice,
            ActualPrice = actualPrice,
            Difference = a.TargetPrice - actualPrice,
            State = a.State
        };
    }
}