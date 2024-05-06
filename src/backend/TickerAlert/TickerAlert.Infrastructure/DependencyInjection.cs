using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Infrastructure.Authentication;
using TickerAlert.Infrastructure.BackgroundJobs;
using TickerAlert.Infrastructure.BackgroundJobs.Helpers;
using TickerAlert.Infrastructure.Common;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterSettings(services, configuration);
        RegisterCommonServices(services);
        services.AddPersistence(configuration);
        services.AddHttpClient();
        RegisterAuthenticationServices(services, configuration);
        RegisterExternalServices(services);
        RegisterBackgroundJobs(services);
        
        return services;
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
}