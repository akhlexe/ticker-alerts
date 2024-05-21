using FluentAssertions;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Domain.UnitTests.Services;

public class PriceTargetEvaluatorTests
{
    private readonly Guid UserId = Guid.NewGuid();
    
    [Theory]
    [InlineData(1000, 900, PriceThresholdType.Below)]
    [InlineData(1000, 1100, PriceThresholdType.Above)]
    public void IsTargetReached_WhenAlertReachTarget_ReturnsTrue(decimal target, decimal measuredPrice, PriceThresholdType threshold)
    {
        // Arrange
        var asset = FinancialAsset.Create(Guid.NewGuid(), "SPY", "Standard & Poor 500");
        var alert = Alert.Create(Guid.NewGuid(), UserId, asset.Id, target, threshold);
        var priceMeasure = PriceMeasure.Create(Guid.NewGuid(), asset.Id, measuredPrice);
        
        // Act
        bool result = PriceTargetEvaluator.IsTargetReached(alert, priceMeasure);

        // Assert
        result.Should().BeTrue();
    }
}