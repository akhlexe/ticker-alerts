namespace TickerAlert.Infrastructure.BackgroundJobs.Helpers;

public static class JobIntervalsInSeconds
{
    public const int ProcessOutboxMessagesJob = 10;
    public const int PriceReaderJob = 300;
}