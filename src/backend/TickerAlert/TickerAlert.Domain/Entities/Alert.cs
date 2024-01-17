using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    public class Alert : Entity
    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public int Direction { get; set; }
    }
}
