using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.FinancialAssets;

public sealed class FinancialAssetProfileService(
    IStockMarketService stockMarketService, 
    IApplicationDbContext _context,
    ICompanyProfileCacheService cacheService)
{
    public async Task<FinancialAssetProfileDto> GetFinancialAssetProfileAsync(Guid financialAssetId)
    {
        FinancialAssetProfileDto? financialAssetProfile = await cacheService.GetCompanyProfileDto(financialAssetId);

        return financialAssetProfile is null
            ? await FetchAndSaveInCacheAsync(financialAssetId)
            : financialAssetProfile;
    }

    private async Task<FinancialAssetProfileDto> FetchAndSaveInCacheAsync(Guid financialAssetId)
    {
        FinancialAsset? financialAsset = await _context
            .FinancialAssets
            .FirstOrDefaultAsync(f => f.Id == financialAssetId);

        if (financialAsset is null) return CreateUnknownAssetResponse();

        Task<CompanyProfileDto> companyProfileTask = stockMarketService.GetCompanyProfile(financialAsset.Ticker);
        Task<Cedear?> cedearTask = _context.Cedears.FirstOrDefaultAsync(x => x.FinancialAssetId == financialAssetId);

        await Task.WhenAll(companyProfileTask, cedearTask);

        CompanyProfileDto companyProfileDto = companyProfileTask.Result;
        Cedear? cedear = cedearTask.Result;

        FinancialAssetProfileDto financialAssetProfile = CreateFinancialAssetProfile(companyProfileDto, cedear);

        await cacheService.SaveCompanyProfileDto(financialAssetId, financialAssetProfile);

        return financialAssetProfile;
    }

    private static FinancialAssetProfileDto CreateFinancialAssetProfile(CompanyProfileDto companyProfileDto, Cedear? cedear)
    {
        return new FinancialAssetProfileDto
        {
            Profile = companyProfileDto,
            CedearInformation = new CedearInformationDto
            {
                Ratio = cedear?.Ratio ?? "Unknown ratio",
                HasCedear = cedear is not null
            }
        };
    }

    private static void CompleteCompanyProfileWithCedearInformation(CompanyProfileDto companyProfileDto, Cedear? cedear)
    {
        if (cedear is not null)
        {
            companyProfileDto.CedearRatio = cedear.Ratio;
            companyProfileDto.HasCedear = true;
        }
    }

    private static FinancialAssetProfileDto CreateUnknownAssetResponse() => new()
    {
        Profile = new CompanyProfileDto
        {
            Ticker = "Unknown Ticker",
            Name = "Unkonwn",
        },
        CedearInformation = new CedearInformationDto
        {
            HasCedear = false,
            Ratio = "Unknown Ratio"
        }
    };
}
