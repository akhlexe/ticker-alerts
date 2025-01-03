
using TickerAlert.Application.Interfaces.Cedears.Dtos;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina.Dtos;

namespace TickerAlert.Application.Services.Cedears;

public class CedearCotizacionService(
    CedearInformationService cedearInformationService,
    IDolarArgentinaCacheService dolarArgentinaCacheService,
    IPriceMeasureReader priceMeasureReader)
{
    public async Task<CedearCotizacion> GetCedearCotizacionAsync(Guid financialAssetId)
    {
        var lastPrice = await priceMeasureReader.GetLastPriceFor(financialAssetId);
        var cedearInformation = await cedearInformationService.GetCedearInformationAsync(financialAssetId);
        var cotizacionDolar = await dolarArgentinaCacheService.GetLastCotizacionDolarCCL();

        return CreateCedearCotizacion(lastPrice, cedearInformation, cotizacionDolar);
    }

    private CedearCotizacion CreateCedearCotizacion(decimal lastPrice, CedearInformationDto cedearInformation, CotizacionDolar? cotizacionDolar)
    {
        if (cedearInformation is null || !cedearInformation.HasCedear || cotizacionDolar is null)
        {
            return CreateEmptyCedearCotizacion();
        }

        decimal ratioNumber = ExtractRatioFromCedearInformation(cedearInformation);
        decimal cedearCompra = lastPrice * cotizacionDolar.Compra / ratioNumber;
        decimal cedearVenta = lastPrice * cotizacionDolar.Venta / ratioNumber;

        return new CedearCotizacion()
        {
            CedearCompra = cedearCompra,
            CedearVenta = cedearVenta,
            Ratio = cedearInformation.Ratio,
            HasCedear = cedearInformation.HasCedear
        };
    }

    private decimal ExtractRatioFromCedearInformation(CedearInformationDto cedearInformation)
    {
        string[] parts = cedearInformation.Ratio.Split(":");

        return decimal.Parse(parts[0]);
    }

    private static CedearCotizacion CreateEmptyCedearCotizacion() 
        => new CedearCotizacion();
}
