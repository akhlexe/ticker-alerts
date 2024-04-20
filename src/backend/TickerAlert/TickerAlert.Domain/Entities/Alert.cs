using TickerAlert.Domain.Common;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Domain.Entities
{
    public class Alert : Entity
    {
        public int FinancialAssetId { get; private set; }
        public decimal TargetPrice { get; private set;} 
        public PriceThresholdType ThresholdType { get; private set;}
        public AlertState State { get; private set; }
        
        public Alert(int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType) 
            : base()
        {
            FinancialAssetId = financialAssetId;
            TargetPrice = targetPrice;
            ThresholdType = thresholdType;
            State = AlertState.PENDING;
        }
        
        public Alert(int id, int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType) 
            : base(id)
        {
            FinancialAssetId = financialAssetId;
            TargetPrice = targetPrice;
            ThresholdType = thresholdType;
        }
        
        public FinancialAsset FinancialAsset { get; set; }
    }
}
