using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Infrastructure.Authentication;
using TickerAlert.Infrastructure.BackgroundJobs;
using TickerAlert.Infrastructure.BackgroundJobs.Helpers;
using TickerAlert.Infrastructure.Cache.Extensions;
using TickerAlert.Infrastructure.Common;
using TickerAlert.Infrastructure.ExternalServices.CotizacionDolares;
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

        services.RegisterRedisCacheService(configuration);
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
            QuartzJobsConfigurator.RegisterTimedJob<CotizacionDolarReaderJob>(config, JobIntervalsInSeconds.CotizacionDolarReaderJob);
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
        services.AddScoped<IDolarArgentinaService, DolarArgentinaService>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }
}