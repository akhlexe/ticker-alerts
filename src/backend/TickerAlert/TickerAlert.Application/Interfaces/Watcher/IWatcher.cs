using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Watcher
{
    public interface IWatcher
    {
        bool IsTargetReached(Alert alert, PriceMeasure measure);
    }
}
