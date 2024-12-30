using Microsoft.Extensions.Configuration;

namespace TickerAlert.Infrastructure.Cache;

internal class RedisConnectionStringFactory
{
    private const string ConnectionStringTemplate = "{host}:{port},password={password}";

    public static string CreateConnectionString(IConfiguration configuration)
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
