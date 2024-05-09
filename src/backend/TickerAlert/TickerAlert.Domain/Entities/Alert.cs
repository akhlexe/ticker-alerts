using TickerAlert.Domain.Common;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Domain.Entities
{
    public class Alert : Entity
    {
        public Guid UserId { get; private set; }
        public Guid FinancialAssetId { get; private set; }
        public decimal TargetPrice { get; private set;} 
        public PriceThresholdType ThresholdType { get; private set;}
        public AlertState State { get; set; }

        public static Alert Create(
            Guid id, 
            Guid userId, 
            Guid financialAssetId,
            decimal targetPrice, 
            PriceThresholdType priceThreshold ) 
            => new(id, userId, financialAssetId, targetPrice, priceThreshold);

        private Alert(Guid id, Guid userId, Guid financialAssetId, decimal targetPrice, PriceThresholdType priceThreshold) 
            : base(id)
        {
            UserId = userId;
            FinancialAssetId = financialAssetId;
            TargetPrice = targetPrice;
            ThresholdType = priceThreshold;
            State = AlertState.PENDING;
        }
        
        public FinancialAsset? FinancialAsset { get; set; }
        public User? User { get; set; }
    }
}
