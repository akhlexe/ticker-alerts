using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    public class FinancialAsset : Entity
    {
        public string Ticker { get; private set; }
        public string Name { get; private set; }

        private FinancialAsset(Guid id, string ticker, string name) : base(id)
        {
            Ticker = ticker;
            Name = name;
        }

        public static FinancialAsset Create(Guid id, string ticker, string name) 
            => new(id, ticker, name);
    }
}
