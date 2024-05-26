namespace TickerAlert.Infrastructure.Persistence.Seeders;

public interface IDataSeeder
{
    Task Seed(IServiceProvider serviceProvider);
}