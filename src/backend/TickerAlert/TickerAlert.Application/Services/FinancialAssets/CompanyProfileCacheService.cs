using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.Services.FinancialAssets;

internal class CompanyProfileCacheService(ICacheService cacheService) : ICompanyProfileCacheService
{
    private const string NamespacePrefix = "CompanyProfile";
    private const int TTL = 3600;

    public async Task<CompanyProfileDto?> GetCompanyProfileDto(Guid financialAssetId) 
        => await cacheService.GetAsync<CompanyProfileDto>(NamespacePrefix, financialAssetId.ToString());

    public async Task SaveCompanyProfileDto(Guid financialAssetId, CompanyProfileDto companyProfileDto)
        => await cacheService.SetAsync(NamespacePrefix, financialAssetId.ToString(), companyProfileDto, TimeSpan.FromMinutes(TTL));
}
