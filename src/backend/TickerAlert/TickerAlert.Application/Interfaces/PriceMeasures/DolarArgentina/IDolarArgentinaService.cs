using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina.Dtos;

namespace TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;

public interface IDolarArgentinaService
{
    Task<CotizacionDolar?> GetCotizacionDolarCCL();
}
