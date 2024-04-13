using FluentAssertions;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Domain.UnitTests.Services;

public class PriceTargetEvaluatorTests
{
    [Theory]
    [InlineData(1000, 900, PriceThresholdType.Below)]
    [InlineData(1000, 1100, PriceThresholdType.Above)]
    public void when_alert_reachs_target_price_returns_true(decimal target, decimal measuredPrice, PriceThresholdType threshold)
    {
        var asset = new FinancialAsset("SPY", "Standard & Poor 500");
        var alert = new Alert(asset.Id, target, threshold);
        var priceMeasure = new PriceMeasure(asset.Id, measuredPrice, DateTime.UtcNow);

        bool result = PriceTargetEvaluator.IsTargetReached(alert, priceMeasure);

        result.Should().Be(true);
    }
}