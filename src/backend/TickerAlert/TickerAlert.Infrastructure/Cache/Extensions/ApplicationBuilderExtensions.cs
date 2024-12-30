using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace TickerAlert.Infrastructure.Cache.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ResetCache(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var redisConnection = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            redisConnection.FlushDb();
        }

        return app;
    }
}
