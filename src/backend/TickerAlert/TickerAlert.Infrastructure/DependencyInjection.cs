using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.AddScoped<FinancialAssetSeeder>();
        services.AddHttpClient();
        
        return services;
    }
}