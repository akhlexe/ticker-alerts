using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.Services.Notifiers;
using TickerAlert.Application.Services.PriceEvaluator;
using TickerAlert.Application.Services.Prices;
using TickerAlert.Application.Services.Watchlists;

namespace TickerAlert.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        RegisterApplicationServices(services);
        RegisterCacheServices(services);
        return services;
    }

    private static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IAlertReader, AlertReader>();
        services.AddScoped<ISystemAlertReader, SystemAlertReader>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IFinancialAssetReader, FinancialAssetReader>();
        services.AddScoped<FinancialAssetProfileService>();
        services.AddScoped<PriceCollectorService>();
        services.AddScoped<PriceEvaluatorService>();
        services.AddScoped<AlertTriggeredNotifier>();
        services.AddScoped<IPriceMeasureService, PriceMeasureService>();
        services.AddScoped<IPriceMeasureReader, PriceMeasureReader>();
        services.AddScoped<IWatchlistService, WatchlistService>();
        
    }

    private static void RegisterCacheServices(IServiceCollection services)
    {
        services.AddScoped<ILastPriceCacheService, LastPriceCacheService>();
        services.AddScoped<ICompanyProfileCacheService, CompanyProfileCacheService>();
    }
}