using FluentAssertions;
using TickerAlert.Application.Services.Watcher;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.UnitTests
{
    public class WatcherTests
    {
        private readonly Watcher _watcher;

        public WatcherTests()
        {
            _watcher = new Watcher();
        }

        [Theory]
        [InlineData(1000, 900, PriceThresholdType.Below)]
        [InlineData(1000, 1100, PriceThresholdType.Above)]
        public void when_alert_reachs_target_price_returns_true(decimal target, decimal measuredPrice, PriceThresholdType threshold)
        {
            var asset = new FinancialAsset("SPY", "Standard & Poor 500");
            var alert = new Alert(asset, target, threshold);
            var priceMeasure = new PriceMeasure() { Asset = asset, Price = measuredPrice, MeasuredOn = DateTime.UtcNow };

            bool result = _watcher.IsTargetReached(alert, priceMeasure);

            result.Should().Be(true);
        }
    }
}