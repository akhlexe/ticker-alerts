using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.Services.FinancialAssets;

internal class CompanyProfileCacheService(ICacheService cacheService) : ICompanyProfileCacheService
{
    private const string NamespacePrefix = "CompanyProfile";
    private const int TTL = 3600;

    public async Task<FinancialAssetProfileDto?> GetCompanyProfileDto(Guid financialAssetId) 
        => await cacheService.GetAsync<FinancialAssetProfileDto>(NamespacePrefix, financialAssetId.ToString());

    public async Task SaveCompanyProfileDto(Guid financialAssetId, FinancialAssetProfileDto financialAssetProfile)
        => await cacheService.SetAsync(NamespacePrefix, financialAssetId.ToString(), financialAssetProfile, TimeSpan.FromMinutes(TTL));
}
