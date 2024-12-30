using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina.Dtos;

namespace TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;

public interface IDolarArgentinaCacheService
{
    Task<CotizacionDolar?> GetLastCotizacionDolarCCL();
    Task UpdateCotizacionDolarCCL(CotizacionDolar cotizacion);
}
