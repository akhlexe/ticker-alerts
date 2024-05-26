namespace TickerAlert.Infrastructure.Persistence.Seeders;

public class ApplicationDataSeeder : IDataSeeder
{
    public async Task Seed(IServiceProvider serviceProvider)
    {
        await FinancialAssetSeeder.Seed(serviceProvider);
    }
}