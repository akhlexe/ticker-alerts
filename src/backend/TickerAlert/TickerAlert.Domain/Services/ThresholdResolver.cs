using TickerAlert.Domain.Enums;

namespace TickerAlert.Domain.Services;

public static class ThresholdResolver
{
    public static PriceThresholdType Resolve(decimal currentPrice, decimal targetPrice)
    {
        return targetPrice > currentPrice 
            ? PriceThresholdType.Above 
            : PriceThresholdType.Below;
    }
}