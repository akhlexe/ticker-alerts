using TickerAlert.Domain.Common;
using TickerAlert.Domain.ValueObjects;

namespace TickerAlert.Domain.Entities
{
    internal class FinancialAsset : Entity
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public Price Price { get; set; }
        public DateTime FromDate { get; set; }
    }
}
