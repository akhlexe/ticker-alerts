using Microsoft.Extensions.Logging;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Services.Prices.MarketHours;

namespace TickerAlert.Application.Services.Prices.DolarArgentina;

public class CotizacionDolarReader(
    IDolarArgentinaService dolarArgentinaService,
    IDolarArgentinaCacheService cacheService,
    ILogger<CotizacionDolarReader> logger)
{
    public async Task ReadCotizacionAsync()
    {
        DateTime now = DateTime.UtcNow;

        // Only for test
        //DateTime fake = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 15, 0, 0, DateTimeKind.Utc); // 3:00 PM UTC

        if (ArgentinaMarketHours.IsMarketOpen(now))
        {
            await ReadCotizacionDolares(dolarArgentinaService, cacheService, logger);
        }
    }

    private static async Task ReadCotizacionDolares(IDolarArgentinaService dolarArgentinaService, IDolarArgentinaCacheService cacheService, ILogger<CotizacionDolarReader> logger)
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
