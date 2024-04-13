using FluentAssertions;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Domain.UnitTests.Services;

public class ThresholdResolverTests
{
    [Theory]
    [InlineData(1000, 900, PriceThresholdType.Below)]
    [InlineData(1000, 1100, PriceThresholdType.Above)]
    public void threshold_should_be_resolved(decimal currentPrice, decimal targetPrice, PriceThresholdType expectedThreshold)
    {
        var resolvedThreshold = ThresholdResolver.Resolve(currentPrice, targetPrice);

        resolvedThreshold.Should().Be(expectedThreshold);
    }
}