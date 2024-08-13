using Microsoft.EntityFrameworkCore;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Api.Utilities;

public static class PrepDB
{
    public static void Migrate(IApplicationBuilder builder)
    {

        using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        int pendingMigrations = context.Database.GetPendingMigrations().Count();

        if (pendingMigrations > 0)
        {
            const bool isProcessing = true;

            while (isProcessing)
            {
                try
                {
                    if (context.Database.GetPendingMigrations().Count() > 0)
                    {
                        context.Database.Migrate();
                        break;
                    }


                }
                catch (Exception ex)
                {
                    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}