using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using StackExchange.Redis;
using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Infrastructure.Authentication;
using TickerAlert.Infrastructure.BackgroundJobs;
using TickerAlert.Infrastructure.BackgroundJobs.Helpers;
using TickerAlert.Infrastructure.Cache;
using TickerAlert.Infrastructure.Common;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService;
using TickerAlert.Infrastructure.Mailing;
using TickerAlert.Infrastructure.NotificationService;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        RegisterSettings(services, configuration);
        RegisterCommonServices(services);
        services.AddPersistence(configuration);
        services.AddHttpClient();
        RegisterAuthenticationServices(services, configuration);
        RegisterExternalServices(services);
        RegisterBackgroundJobs(services);
        RegisterNotificationServices(services);
        RegisterRedisCacheService(services, configuration);

        services.RegisterTickerbloomEmailService(configuration, environment);

        return services;
    }

    private static void RegisterNotificationServices(IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<INotificationService, SignalRNotificationService>();
        services.AddSingleton<IUserIdProvider, SignalrUserIdProvider>();
    }

    private static void RegisterBackgroundJobs(IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            QuartzJobsConfigurator.RegisterTimedJob<ProcessOutboxMessagesJob>(config, JobIntervalsInSeconds.ProcessOutboxMessagesJob);
            QuartzJobsConfigurator.RegisterTimedJob<PriceReaderJob>(config, JobIntervalsInSeconds.PriceReaderJob);
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    private static void RegisterAuthenticationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuthentication(configuration);
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    private static void RegisterCommonServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }

    private static void RegisterExternalServices(IServiceCollection services)
    {
        services.AddScoped<IStockMarketService, StockMarketService>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }

    private static void RegisterRedisCacheService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string connection = configuration["Redis:Connection"] 
                ?? throw new ArgumentNullException("Redis connection is missing.");

            var configurationOptions = ConfigurationOptions.Parse(connection, true);

            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        services.AddScoped<ICacheService, RedisCacheService>();
    }
}