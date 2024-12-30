using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;

namespace TickerAlert.Application.Services.Prices.DolarArgentina;

public class CotizacionDolarReader(
    IDolarArgentinaService dolarArgentinaService,
    IDolarArgentinaCacheService cacheService)
{
    public async Task ReadCotizacionAsync()
    {
        var cotizacionDolar = await dolarArgentinaService.GetCotizacionDolarCCL();

        if (cotizacionDolar is not null)
        {
            await cacheService.UpdateCotizacionDolarCCL(cotizacionDolar);
        }
    }
}
