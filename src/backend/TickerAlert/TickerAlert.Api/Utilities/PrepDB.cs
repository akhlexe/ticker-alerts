using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Api.Utilities;

public static class PrepDB
{
    public static void Migrate(IApplicationBuilder builder)
    {

        using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        const bool isProcessing = true;

        while(isProcessing)
        {
            try
            {
                context.Database.Migrate();
                break;
            }
            catch(SqlException ex)
            {
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                Console.WriteLine(ex.Message);
            }
        }
    }
}