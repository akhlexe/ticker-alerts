using Microsoft.Extensions.Logging;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;

namespace TickerAlert.Application.Services.Prices.DolarArgentina;

public class CotizacionDolarReader(
    IDolarArgentinaService dolarArgentinaService,
    IDolarArgentinaCacheService cacheService,
    ILogger<CotizacionDolarReader> logger)
{
    public async Task ReadCotizacionAsync()
    {
        var cotizacionDolar = await dolarArgentinaService.GetCotizacionDolarCCL();

        if (cotizacionDolar is null)
        {
            logger.LogWarning("External Service: DolarApi is returning null for Cotizacion Dolar CCL.");
            return;
        }

        await cacheService.UpdateCotizacionDolarCCL(cotizacionDolar);
    }
}
