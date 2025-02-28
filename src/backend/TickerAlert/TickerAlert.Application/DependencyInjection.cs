using Microsoft.Extensions.DependencyInjection;
using System;
using TickerAlert.Application.Common.EventBus;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Cedears;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.Services.Cedears;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.CompanyProfile;
using TickerAlert.Application.Services.Notifiers;
using TickerAlert.Application.Services.PriceEvaluator;
using TickerAlert.Application.Services.Prices;
using TickerAlert.Application.Services.Prices.DolarArgentina;
using TickerAlert.Application.Services.Prices.DolarArgentina.Cache;
using TickerAlert.Application.Services.Prices.PriceUpdates;
using TickerAlert.Application.Services.Watchlists;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        RegisterApplicationServices(services);
        return services;
    }

    private static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IAlertReader, AlertReader>();
        services.AddScoped<ISystemAlertReader, SystemAlertReader>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IFinancialAssetReader, FinancialAssetReader>();
        services.AddScoped<FinancialAssetProfileService>();
        services.AddScoped<PriceEvaluatorService>();
        services.AddScoped<AlertTriggeredNotifier>();
        services.AddScoped<IWatchlistService, WatchlistService>();

        RegisterPriceServices(services);
        RegisterCotizacionDolarServices(services);
        RegisterCedearServices(services);
        RegisterCompanyProfileServices(services);
        RegisterConsumers(services);
    }

    private static void RegisterPriceServices(IServiceCollection services)
    {
        services.AddScoped<PriceCollectorService>();
        services.AddScoped<ILastPriceCacheService, LastPriceCacheService>();
        services.AddScoped<IPriceMeasureService, PriceMeasureService>();
        services.AddScoped<IPriceMeasureReader, PriceMeasureReader>();
    }

    private static void RegisterCotizacionDolarServices(IServiceCollection services)
    {
        services.AddScoped<CotizacionDolarReader>();
        services.AddScoped<IDolarArgentinaCacheService, DolarArgentinaCacheService>();
    }

    private static void RegisterCedearServices(IServiceCollection services)
    {
        services.AddScoped<CedearCotizacionService>();
        services.AddScoped<CedearInformationService>();
        services.AddScoped<ICedearInformationCacheService, CedearInformationCacheService>();
    }

    private static void RegisterCompanyProfileServices(IServiceCollection services)
    {
        services.AddScoped<CompanyProfileService>();
        services.AddScoped<ICompanyProfileCacheService, CompanyProfileCacheService>();
    }

    private static void RegisterConsumers(IServiceCollection services)
    {
        services.AddScoped<IEventConsumer<PriceUpdateEvent>, PriceUpdateConsumer>();
        services.AddScoped<IEventConsumer<AlertTriggeredEvent>, AlertTriggeredConsumer>();
    }
}