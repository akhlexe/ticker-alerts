using TickerAlert.Application.Services.Cedears;
using TickerAlert.Application.Services.FinancialAssets.CompanyProfile;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.Services.FinancialAssets;

public sealed class FinancialAssetProfileService(
    CompanyProfileService companyProfileService,
    CedearCotizacionService cedearCotizacionService)
{
    public async Task<FinancialAssetProfileDto> GetFinancialAssetProfileAsync(Guid financialAssetId)
    {
        var companyProfile = await companyProfileService.GetCompanyProfileAsync(financialAssetId);
        var cedearCotizacion = await cedearCotizacionService.GetCedearCotizacionAsync(financialAssetId);

        return new FinancialAssetProfileDto
        {
            Profile = companyProfile,
            CedearCotizacion = cedearCotizacion
        };
    }
}
