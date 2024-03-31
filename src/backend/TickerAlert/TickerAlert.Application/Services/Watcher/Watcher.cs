using TickerAlert.Application.Interfaces.Watcher;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Watcher
{
    public class Watcher : IWatcher
    {
        public bool IsTargetReached(Alert alert, PriceMeasure measure)
        {
            throw new NotImplementedException();
        }
    }
}
