using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.Interfaces.FinancialAssets;

public interface ICompanyProfileCacheService
{
    Task<CompanyProfileDto?> GetCompanyProfileDto(Guid financialAssetId);
    Task SaveCompanyProfileDto(Guid financialAssetId, CompanyProfileDto companyProfileDto);
}
