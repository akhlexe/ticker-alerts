using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.PriceMeasures;

public interface IPriceMeasureService
{
    Task ProcessPriceMeasure(PriceMeasure measure);
}