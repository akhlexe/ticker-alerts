using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.FinancialAssets;

public sealed class FinancialAssetProfileService(
    IStockMarketService stockMarketService, 
    IApplicationDbContext _context)
{
    public async Task<CompanyProfileDto> GetFinancialAssetProfileAsync(Guid financialAssetId)
    {
        FinancialAsset? financialAsset = await _context.FinancialAssets.FirstOrDefaultAsync(f => f.Id == financialAssetId);

        if (financialAsset is null) return CreateUnknownAssetResponse();

        return await stockMarketService.GetCompanyProfile(financialAsset.Ticker);
    }

    private static CompanyProfileDto CreateUnknownAssetResponse() => new()
    {
        Ticker = "Unknown Ticker",
        Name = "Unkonwn",
    };
}
