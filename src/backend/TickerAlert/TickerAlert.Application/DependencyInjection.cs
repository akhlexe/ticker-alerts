using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.Watcher;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.Services.Watcher;

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
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IFinancialAssetReader, FinancialAssetReader>();
        services.AddScoped<IWatcher, Watcher>();
    }
}