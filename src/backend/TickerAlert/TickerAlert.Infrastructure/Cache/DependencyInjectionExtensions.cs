using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TickerAlert.Application.Common.Cache;

namespace TickerAlert.Infrastructure.Cache;

public static class DependencyInjectionExtensions
{
    public static void RegisterRedisCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string connection = RedisConnectionStringFactory.CreateConnectionString(configuration);

            return ConnectionMultiplexer.Connect(connection);
        });

        services.AddScoped<ICacheService, RedisCacheService>();
    }
}
