using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TickerAlert.Infrastructure.Persistence;

public static class PersistenceDependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options 
            => options.UseSqlServer(configuration.GetConnectionString("TickerAlertsDatabase")));
    }
}