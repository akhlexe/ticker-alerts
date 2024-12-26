using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Alerts;

public class AlertReader(
    IApplicationDbContext context,
    IPriceMeasureReader priceMeasureReader,
    ICurrentUserService currentUserService) : IAlertReader
{
    private static List<AlertState> EstadosVisibles => [AlertState.PENDING, AlertState.TRIGGERED, AlertState.NOTIFIED];

    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        var alerts = await GetAllForUserId(currentUserService.UserId);
        var lastPrices = await priceMeasureReader.GetLastPricesFor(alerts.Select(x => x.FinancialAssetId));

        return alerts.Select(alert => CreateAlertDto(alert, lastPrices.GetValueOrDefault(alert.FinancialAssetId)));
    }

    public async Task<Alert?> GetById(Guid alertId) 
        => await context
            .Alerts
            .Include(x => x.FinancialAsset)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == alertId);

    public async Task<IEnumerable<Alert>> GetAllForUserId(Guid userId) 
        => await context
            .Alerts
            .Include(a => a.FinancialAsset)
            .Where(a => a.UserId == userId && EstadosVisibles.Contains(a.State))
            .AsNoTracking()
            .ToListAsync();

    private static AlertDto CreateAlertDto(Alert a, decimal lastPrice)
    {
        return new AlertDto()
        {
            Id = a.Id,
            FinancialAssetId = a.FinancialAssetId,
            TickerName = a.FinancialAsset.Ticker,
            TargetPrice = a.TargetPrice,
            ActualPrice = lastPrice,
            Difference = a.TargetPrice - lastPrice,
            State = a.State
        };
    }
}