using TickerAlert.Application.Interfaces.Watcher;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Watcher
{
    public class Watcher : IWatcher
    {
        public bool IsTargetReached(Alert alert, PriceMeasure measure)
        {
            return alert.ThresholdType switch
            {
                PriceThresholdType.Above => HasPriceReachOverAlertTarget(alert, measure),
                PriceThresholdType.Below => HasPriceGoesBelowAlertTarget(alert, measure),
                _ => throw new NotSupportedException("PriceThresholdType not supported.")
            };
        }

        private static bool HasPriceGoesBelowAlertTarget(Alert alert, PriceMeasure measure)
        {
            return measure.Price <= alert.TargetPrice;
        }

        private static bool HasPriceReachOverAlertTarget(Alert alert, PriceMeasure measure)
        {
            return measure.Price >= alert.TargetPrice;
        }
    }
}
