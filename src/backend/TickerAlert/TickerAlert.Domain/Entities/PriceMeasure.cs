using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities
{
    public class PriceMeasure : Entity
    {
        public int FinancialAssetId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime MeasuredOn { get; private set; }

        public PriceMeasure(int financialAssetId, decimal price, DateTime measuredOn) 
            : base()
        {
            FinancialAssetId = financialAssetId;
            Price = price;
            MeasuredOn = measuredOn;
        }
        
        public PriceMeasure(int id, int financialAssetId, decimal price, DateTime measuredOn) 
            : base(id)
        {
            FinancialAssetId = financialAssetId;
            Price = price;
            MeasuredOn = measuredOn;
        }
        
        public FinancialAsset FinancialAsset { get; private set; }
    }
}
