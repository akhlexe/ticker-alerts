using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence.Seeders.Dtos;

namespace TickerAlert.Infrastructure.Persistence.Seeders;
/// <summary>
/// Seeder for filling stocks available data for NASDAQ
/// </summary>
public static class FinancialAssetSeeder
{
    private const string SeedUrl = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token={API_KEY}";
    private const int BATCH_SIZE = 1000;

    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var configuration = services.GetRequiredService<IConfiguration>();
        var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (!context.FinancialAssets.Any())
        {
            await SeedData(configuration, httpClientFactory, context);
        }
    }

    private static async Task SeedData(IConfiguration configuration, IHttpClientFactory httpFactory, ApplicationDbContext context)
    {
        string apiKey = configuration["Services:Finnhub:ApiKey"];
        var queryUrl = SeedUrl.Replace("{API_KEY}", apiKey);

        var httpClient = httpFactory.CreateClient();
        var financialAssetDtos = await httpClient.GetFromJsonAsync<IEnumerable<FinancialAssetSeedDto>>(queryUrl);
        
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        var financialAssets = financialAssetDtos
            .Select(dto => FinancialAsset.Create(Guid.NewGuid(), dto.Symbol, dto.Description))
            .ToList();

        LoadData(context, financialAssets);
    }

    private static void LoadData(ApplicationDbContext context, List<FinancialAsset> financialAssets)
    {
        int count = 0;
        
        foreach (var financialAsset in financialAssets)
        {
            context.FinancialAssets.Add(financialAsset);
            count++;
            
            if (count % BATCH_SIZE == 0)
            {
                context.SaveChanges();
                context.ChangeTracker.Clear();
            }
        }

        context.SaveChanges();
    }
}