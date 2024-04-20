using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddHttpClient();

        RegisterExternalServices(services);
        
        return services;
    }

    private static void RegisterExternalServices(IServiceCollection services)
    {
        services.AddScoped<IStockMarketService, StockMarketService>();
    }
}