using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TickerAlert.Api.Utilities;
using TickerAlert.Infrastructure.NotificationService;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        PrepDB.Migrate(app);

        var dataSeeder = app.Services.GetRequiredService<IDataSeeder>();

        dataSeeder.Seed(app.Services).GetAwaiter().GetResult();

        return app;
    }

    public static WebApplication AddSwaggerIfDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
    
    public static void Migrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            // Log and potentially halt startup
            Console.Write(ex.Message);
            throw;
        }
    }

    public static WebApplication AddHubEndpoints(this WebApplication app)
    {
        app.UseEndpoints(endpoints => endpoints.MapHub<TickerbloomHub>("tickerbloomhub"));

        return app;
    }
}