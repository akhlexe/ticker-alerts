using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.Interfaces.FinancialAssets;

public interface ICompanyProfileCacheService
{
    Task<FinancialAssetProfileDto?> GetCompanyProfileDto(Guid financialAssetId);
    Task SaveCompanyProfileDto(Guid financialAssetId, FinancialAssetProfileDto companyProfileDto);
}
