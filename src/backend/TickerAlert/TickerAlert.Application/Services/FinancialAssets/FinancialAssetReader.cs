using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.FinancialAssets;

public class FinancialAssetReader : IFinancialAssetReader
{
    private readonly IApplicationDbContext _context;

    public FinancialAssetReader(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialAssetDto>> GetAllBySearchCriteria(string criteria)
    {
        var assets = await _context.FinancialAssets
            .Where(x => x.Name.Contains(criteria) || x.Ticker.Contains(criteria))
            .ToListAsync();

        return assets.Select(CreateFinancialAssetDto);
    }

    public async Task<IEnumerable<FinancialAsset>> GetAllWithPendingAlerts()
    {
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