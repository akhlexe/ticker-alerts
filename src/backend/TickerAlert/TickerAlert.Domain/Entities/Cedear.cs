using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

/// <summary>
/// Derivado financiero argentino para poder operar activos extranjeros.
/// </summary>
public class Cedear : Entity
{
    public Guid FinancialAssetId { get; private set; }
    public string Ratio { get; private set; }

    private Cedear(Guid id, Guid financialAssetId, string ratio) : base(id)
    {
        FinancialAssetId = financialAssetId;
        Ratio = ratio;
    }

    public static Cedear Create(Guid id, Guid financialAssetId, string ratio)
        => new(id, financialAssetId, ratio);

    public FinancialAsset? FinancialAsset { get; private set; }
}
