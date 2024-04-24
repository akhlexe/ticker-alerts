using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Infrastructure.Authentication;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterSettings(services, configuration);
        services.AddPersistence(configuration);
        services.AddHttpClient();
        RegisterAuthenticationServices(services, configuration);
        RegisterExternalServices(services);
        
        return services;
    }
    
    private static void RegisterAuthenticationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuthentication(configuration);
        services.AddScoped<IAuthenticationService, AuthenticationService>();
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