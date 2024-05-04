using Quartz;

namespace TickerAlert.Infrastructure.BackgroundJobs.Helpers;

public static class QuartzJobsConfigurator
{
    public static void RegisterTimedJob<TJob>(IServiceCollectionQuartzConfigurator quartz, int intervalInSeconds)
        where TJob : IJob
    {
        var jobKey = new JobKey(typeof(TJob).Name);

        quartz.AddJob<TJob>(opts => opts.WithIdentity(jobKey))
            .AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule => schedule
                    .WithIntervalInSeconds(intervalInSeconds)
                    .RepeatForever()));
    }
}