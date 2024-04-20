using TickerAlert.Application.Interfaces.Watcher;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Watcher
{
    /// <summary>
    /// TODO: Re design this class.
    /// </summary>
    public class Watcher : IWatcher
    {
        public bool IsTargetReached(Alert alert, PriceMeasure measure)
        {
            throw new NotImplementedException();
        }
    }
}
