using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.FinancialAssets;

public class FinancialAssetReader(IApplicationDbContext context) : IFinancialAssetReader
{
    public async Task<IEnumerable<FinancialAssetDto>> GetAllBySearchCriteria(string criteria)
    {
        var normalizedCriteria = criteria.Trim().ToLowerInvariant();

        var assets = await context.FinancialAssets
            .Where(x => x.Name.ToLower().Contains(normalizedCriteria) || x.Ticker.ToLower().Contains(normalizedCriteria))
            .ToListAsync();

        return assets.Select(CreateFinancialAssetDto);
    }

    public async Task<IEnumerable<FinancialAsset>> GetAllWithPendingAlerts()
    {
        return await context.FinancialAssets
            .Join(context.Alerts,
                fa => fa.Id,
                alert => alert.FinancialAssetId,
                (fa, alert) => new { FinancialAsset = fa, Alert = alert })
            .Where(faAlert => faAlert.Alert.State == AlertState.PENDING)
            .Select(faAlert => faAlert.FinancialAsset)
            .Distinct()
            .ToListAsync();
    }

    private static FinancialAssetDto CreateFinancialAssetDto(FinancialAsset financialAsset)
    {
        return new FinancialAssetDto()
        {
            Id = financialAsset.Id,
            Ticker = financialAsset.Ticker,
            Name = financialAsset.Name,
        };
    }
}