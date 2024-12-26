using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TickerAlert.Application.Common.Cache;

namespace TickerAlert.Infrastructure.Cache;

public static class DependencyInjectionExtensions
{
    private const string ConnectionStringTemplate = "{host}:{port},password={password}";

    public static void RegisterRedisCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string connection = CreateConnectionStringFromConfiguration(configuration);

            return ConnectionMultiplexer.Connect(connection);
        });

        services.AddScoped<ICacheService, RedisCacheService>();
    }

    private static string CreateConnectionStringFromConfiguration(IConfiguration configuration)
    {
        string host = configuration["Redis:Host"]
            ?? throw new ArgumentNullException("Redis host is missing.");
        string port = configuration["Redis:Port"]
            ?? throw new ArgumentNullException("Redis port is missing.");
        string password = configuration["Redis:Password"]
            ?? throw new ArgumentNullException("Redis password is missing.");

        return ConnectionStringTemplate
            .Replace("{host}", host)
            .Replace("{port}", port)
            .Replace("{password}", password);
    }
}
