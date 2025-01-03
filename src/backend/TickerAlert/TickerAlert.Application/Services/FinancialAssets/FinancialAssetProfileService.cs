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
        var companyProfileTask = companyProfileService.GetCompanyProfileAsync(financialAssetId);
        var cedearCotizacionTask = cedearCotizacionService.GetCedearCotizacionAsync(financialAssetId);

        await Task.WhenAll(companyProfileTask, cedearCotizacionTask);

        var companyProfile = companyProfileTask.Result;
        var cedearCotizacion = cedearCotizacionTask.Result;

        return new FinancialAssetProfileDto
        {
            Profile = companyProfile,
            CedearCotizacion = cedearCotizacion
        };
    }
}
