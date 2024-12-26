using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.IntegrationTests.SeedDatabase;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Infrastructure.Cache;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Interceptors;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Application.IntegrationTests.Common;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.2.3")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveServicesToBeReplaced(services);

            RegisterPosgresContainer(services);
            RegisterRedisContainer(services);
        });
    }

    private void RegisterPosgresContainer(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            options.UseNpgsql(_dbContainer.GetConnectionString())
                .AddInterceptors(interceptor);
        });

        services.AddSingleton<IDataSeeder, TestDataSeeder>();
        services.AddSingleton<ICurrentUserService, CurrentUserServiceFake>();
    }

    private void RegisterRedisContainer(IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string connection = _redisContainer.GetConnectionString()
                ?? throw new ArgumentNullException("Redis connection is missing.");

            var configurationOptions = ConfigurationOptions.Parse(connection, true);

            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        services.AddScoped<ICacheService, RedisCacheService>();
    }

    private static void RemoveServicesToBeReplaced(IServiceCollection services)
    {
        var serviceTypesToRemove = new List<Type>
        {
            typeof(DbContextOptions<ApplicationDbContext>),
            typeof(IDataSeeder),
            typeof(ICurrentUserService),
            typeof(IConnectionMultiplexer),
            typeof(ICacheService)
        };

        foreach (var serviceType in serviceTypesToRemove)
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == serviceType);
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
        }
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}