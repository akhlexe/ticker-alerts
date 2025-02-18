namespace TickerAlert.Application.Services.Prices.MarketHours;

public sealed class UnitedStatesMarketHours
{
    // 2:30 PM UTC
    public const string MarketOpenTimeUtc = "14:00";

    // 9:00 PM UTC
    public const string MarketCloseTimeUtc = "21:30";

    public static bool IsMarketOpen(DateTime now)
    {
        DateTime marketOpen = CalculateUtcMarketOpen(now.Date);
        DateTime marketClose = CalculateUtcMarketClose(now.Date);

        return !IsWeekend(now)
            && now >= marketOpen
            && now <= marketClose;
    }

    private static bool IsWeekend(DateTime now)
    {
        return now.DayOfWeek == DayOfWeek.Saturday
            || now.DayOfWeek == DayOfWeek.Sunday;
    }

    private static DateTime CalculateUtcMarketClose(DateTime today) 
        => DateTime.Parse($"{today:yyyy-MM-dd}T{MarketCloseTimeUtc}:00Z");

    private static DateTime CalculateUtcMarketOpen(DateTime today) 
        => DateTime.Parse($"{today:yyyy-MM-dd}T{MarketOpenTimeUtc}:00Z");
}
