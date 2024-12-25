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
            string connection = configuration["Redis:Connection"]
                ?? throw new ArgumentNullException("Redis connection is missing.");

            var configurationOptions = CreateRedisConfigurationOptions(connection);

            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        services.AddScoped<ICacheService, RedisCacheService>();
    }

    private static ConfigurationOptions CreateRedisConfigurationOptions(string connection)
    {
        if (connection.StartsWith("redis://", StringComparison.OrdinalIgnoreCase))
        {
            // Parse redis:// connection string
            var uri = new Uri(connection);

            return new ConfigurationOptions
            {
                EndPoints = { $"{uri.Host}:{uri.Port}" },
                Password = uri.UserInfo.Split(':')[1] // Extract password
            };
        }
        else
        {
            return ConfigurationOptions.Parse(connection, true);
        }
    }
}
