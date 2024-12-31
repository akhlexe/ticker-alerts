using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.FinancialAssets.CompanyProfile;

public class CompanyProfileService(
    IApplicationDbContext context,
    CompanyProfileCacheService cacheService,
    IStockMarketService stockMarketService)
{
    public async Task<CompanyProfileDto> GetCompanyProfileAsync(Guid financialAssetId)
    {
        CompanyProfileDto? companyProfileDto = await cacheService.GetCompanyProfileDto(financialAssetId);

        return companyProfileDto is null
            ? await FetchAndSaveInCacheAsync(financialAssetId)
            : companyProfileDto;
    }

    private async Task<CompanyProfileDto> FetchAndSaveInCacheAsync(Guid financialAssetId)
    {
        FinancialAsset? financialAsset = await context
            .FinancialAssets
            .FirstOrDefaultAsync(f => f.Id == financialAssetId);

        if (financialAsset is null) return CreateUnknownAssetResponse();

        CompanyProfileDto companyProfile = await stockMarketService.GetCompanyProfile(financialAsset.Ticker);

        await cacheService.SaveCompanyProfileDto(financialAssetId, companyProfile);

        return companyProfile;
    }

    private CompanyProfileDto CreateUnknownAssetResponse() 
        => new CompanyProfileDto { Ticker = "Unknown Ticker", Name = "Unkonwn" };
}
