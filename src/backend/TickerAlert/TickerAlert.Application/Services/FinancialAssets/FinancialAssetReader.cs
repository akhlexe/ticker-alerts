using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

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