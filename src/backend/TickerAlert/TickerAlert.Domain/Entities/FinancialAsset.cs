using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    public class FinancialAsset : Entity
    {
        public string Ticker { get; private set; }
        public string Name { get; private set; }

        public FinancialAsset(string ticker, string name) : base()
        {
            this.Ticker = ticker;
            this.Name = name;
        }
        
        public FinancialAsset(int id, string ticker, string name)
            : base(id)
        {
            Ticker = ticker;
            Name = name;
        }
    }
}
