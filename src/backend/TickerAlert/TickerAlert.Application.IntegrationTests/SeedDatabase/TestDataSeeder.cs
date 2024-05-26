using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.IntegrationTests.SeedDatabase.TestData;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Seeders;

namespace TickerAlert.Application.IntegrationTests.SeedDatabase;

public class TestDataSeeder : IDataSeeder
{
    public Task Seed(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        SeedDatabase(dbContext).GetAwaiter().GetResult();

        return Task.CompletedTask;
    }
    
    public static async Task SeedDatabase(ApplicationDbContext context)
    {
        if (!context.Users.Any(x => x.Id == Guid.Parse("e2b0b4e1-21c7-4d2b-b45c-9c2b9a9f4e2a")))
        {
            context.Users.Add(Users.CreateTestUser());
        }

        if (!context.FinancialAssets.Any())
        {
            context.FinancialAssets.AddRange(FinancialAssets.CreateFinancialAssets());
        }

        await context.SaveChangesAsync();
    }
}