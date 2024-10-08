using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Infrastructure.Persistence.Interceptors;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Infrastructure.Persistence;

public static class PersistenceDependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseNpgsql(configuration.GetConnectionString("TickerAlertsDatabase"))
                .AddInterceptors(interceptor);
        });

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddSingleton<IDataSeeder, ApplicationDataSeeder>();
    }
}