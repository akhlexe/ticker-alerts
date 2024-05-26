using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using TickerAlert.Application.IntegrationTests.SeedDatabase;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Interceptors;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Application.IntegrationTests.Common;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveServicesToBeReplaced(services);

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                options.UseSqlServer(_dbContainer.GetConnectionString())
                    .AddInterceptors(interceptor);
            });

            services.AddSingleton<IDataSeeder, TestDataSeeder>();
            services.AddSingleton<ICurrentUserService, CurrentUserServiceFake>();
        });
    }

    private static void RemoveServicesToBeReplaced(IServiceCollection services)
    {
        var serviceTypesToRemove = new List<Type>
        {
            typeof(DbContextOptions<ApplicationDbContext>),
            typeof(IDataSeeder),
            typeof(ICurrentUserService)
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

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}