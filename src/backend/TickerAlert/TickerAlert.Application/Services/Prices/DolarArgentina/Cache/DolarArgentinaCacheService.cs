using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina.Dtos;

namespace TickerAlert.Application.Services.Prices.DolarArgentina.Cache;

public class DolarArgentinaCacheService(ICacheService cacheService) : IDolarArgentinaCacheService
{
    private const string NamespacePrefix = "Cotizaciones";
    private const int ExpirationTime = 600;

    public async Task<CotizacionDolar?> GetLastCotizacionDolarCCL() 
        => await cacheService.GetAsync<CotizacionDolar?>(NamespacePrefix, DolarArgentinaCacheKeys.DolarCCL);

    public async Task UpdateCotizacionDolarCCL(CotizacionDolar cotizacion) 
        => await cacheService.SetAsync(
            NamespacePrefix,
            DolarArgentinaCacheKeys.DolarCCL,
            cotizacion,
            TimeSpan.FromSeconds(ExpirationTime)
        );
}
