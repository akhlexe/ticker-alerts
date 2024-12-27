using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.FinancialAssets;

public sealed class FinancialAssetProfileService(
    IStockMarketService stockMarketService, 
    IApplicationDbContext _context,
    ICompanyProfileCacheService cacheService)
{
    public async Task<CompanyProfileDto> GetFinancialAssetProfileAsync(Guid financialAssetId)
    {
        CompanyProfileDto? companyProfileDto = await cacheService.GetCompanyProfileDto(financialAssetId);

        return companyProfileDto is null
            ? await FetchAndSaveInCacheAsync(financialAssetId)
            : companyProfileDto;
    }

    private async Task<CompanyProfileDto> FetchAndSaveInCacheAsync(Guid financialAssetId)
    {
        FinancialAsset? financialAsset = await _context
            .FinancialAssets
            .FirstOrDefaultAsync(f => f.Id == financialAssetId);

        if (financialAsset is null) return CreateUnknownAssetResponse();

        CompanyProfileDto companyProfileDto = await stockMarketService.GetCompanyProfile(financialAsset.Ticker);

        await cacheService.SaveCompanyProfileDto(financialAssetId, companyProfileDto);

        return companyProfileDto;
    }

    private static CompanyProfileDto CreateUnknownAssetResponse() => new()
    {
        Ticker = "Unknown Ticker",
        Name = "Unkonwn",
    };
}
