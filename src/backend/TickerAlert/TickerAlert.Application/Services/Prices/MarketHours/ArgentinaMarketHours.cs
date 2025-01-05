namespace TickerAlert.Application.Services.Prices.MarketHours;

internal sealed class ArgentinaMarketHours
{
    public const string MarketOpenTimeUtc = "14:00"; // 2:00 PM UTC
    public const string MarketCloseTimeUtc = "20:00"; // 8:00 PM UTC

    public static bool IsMarketOpen()
    {
        DateTime now = DateTime.UtcNow;

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
