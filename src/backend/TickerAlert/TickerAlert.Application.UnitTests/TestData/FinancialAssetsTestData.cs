using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UnitTests.TestData;

public class FinancialAssetsTestData
{
    public static List<FinancialAsset> GetFinancialAssets()
    {
        return new List<FinancialAsset>
        {
            FinancialAsset.Create(Guid.NewGuid(), "ABC", "Example Corp"),
            FinancialAsset.Create(Guid.NewGuid(), "XYZ", "Example Inc")
        };
    }
}