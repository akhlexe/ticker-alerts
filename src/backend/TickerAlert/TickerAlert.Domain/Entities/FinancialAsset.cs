using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    internal class FinancialAsset : Entity
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime FromDate { get; set; }
    }
}
