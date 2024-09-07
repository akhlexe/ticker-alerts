using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class PriceMeasure : Entity
{
    public Guid FinancialAssetId { get; private set; }
    public decimal Price { get; private set; }
    public DateTime MeasuredOn { get; private set; }

    private PriceMeasure(Guid id, Guid financialAssetId, decimal price) : base(id)
    {
        FinancialAssetId = financialAssetId;
        Price = price;
        MeasuredOn = DateTime.UtcNow;
    }

    public static PriceMeasure Create(Guid id, Guid financialAssetId, decimal price) 
        => new(id, financialAssetId, price);

    public FinancialAsset? FinancialAsset { get; private set; }
    
    // EF Core required.
    // private PriceMeasure() : base(Guid.Empty) { }
}
