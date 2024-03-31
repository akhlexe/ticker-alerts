using TickerAlert.Domain.Common;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Domain.Entities
{
    public class Alert : Entity
    {
        public int FinancialAssetId { get; }
        public decimal TargetPrice { get; } 
        public PriceThresholdType ThresholdType { get; }

        public Alert(FinancialAsset asset, decimal targetPrice, PriceThresholdType thresholdType)
        {
            FinancialAssetId = asset.Id;
            TargetPrice = targetPrice;
            ThresholdType = thresholdType;
            FinancialAsset = asset;
        }
        public FinancialAsset FinancialAsset { get; set; }
    }
}
