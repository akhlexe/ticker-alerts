using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Common.Responses;
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
        var normalizedCriteria = criteria.Trim().ToLowerInvariant();

        var assets = await _context
            .FinancialAssets
            .Where(x => x.Name.ToLower().Contains(normalizedCriteria) || x.Ticker.ToLower().Contains(normalizedCriteria))
            .ToListAsync();

        return assets
            .Select(CreateFinancialAssetDto)
            .ToList();
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

    public async Task<Result<FinancialAssetDto>> GetById(Guid id)
    {
        FinancialAsset? financialAsset = await _context
            .FinancialAssets
            .FirstOrDefaultAsync(f => f.Id == id);

        return financialAsset is null
            ? Result<FinancialAssetDto>.FailureResult($"Financial Asset doesnt exists with Id: {id}")
            : Result<FinancialAssetDto>.SuccessResult(CreateFinancialAssetDto(financialAsset));
    }

    public async Task<List<FinancialAssetDto>> GetAllByIds(IEnumerable<Guid> ids)
    {
        List<FinancialAsset> financialAssets = await _context
            .FinancialAssets
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();

        return financialAssets
            .Select(CreateFinancialAssetDto)
            .ToList();
    }
}