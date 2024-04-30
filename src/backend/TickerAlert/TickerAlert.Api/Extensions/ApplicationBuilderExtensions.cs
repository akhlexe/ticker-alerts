using Microsoft.EntityFrameworkCore;
using TickerAlert.Api.Utilities;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        PrepDB.Migrate(app);
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
}