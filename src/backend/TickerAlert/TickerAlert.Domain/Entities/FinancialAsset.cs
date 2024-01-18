using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    public class FinancialAsset : Entity
    {
        public string Ticker { get; }
        public string Name { get; }

        public FinancialAsset(string ticker, string name)
        {
            this.Ticker = ticker;
            this.Name = name;
        }
    }
}
