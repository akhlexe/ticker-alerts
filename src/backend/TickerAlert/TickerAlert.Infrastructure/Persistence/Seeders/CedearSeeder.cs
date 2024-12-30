using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence.Seeders.Dtos;

namespace TickerAlert.Infrastructure.Persistence.Seeders;

public static class CedearSeeder
{
    public static string CedearSourceFilePath => Path.Combine(AppContext.BaseDirectory,"Persistence","Seeders","Assets","cedears.csv");

    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (!context.Cedears.Any())
        {
            var cedears = ReadCedearsFromCsv(CedearSourceFilePath);
            await SeedCedears(context, cedears);
        }
    }

    private static Dictionary<string, string> ReadCedearsFromCsv(string cedearSourceFilePath)
    {
        if (!File.Exists(CedearSourceFilePath))
        {
            string testPath = CedearSourceFilePath;
            throw new FileNotFoundException($"The CSV file '{CedearSourceFilePath}' does not exist.");
        }

        using var reader = new StreamReader(CedearSourceFilePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

        IEnumerable<CedearSeedDto> cedearsFromCsv = csv.GetRecords<CedearSeedDto>();

        return cedearsFromCsv.ToDictionary(x => x.Ticker, x => x.Ratio);
    }

    private static async Task SeedCedears(ApplicationDbContext context, Dictionary<string, string> cedearsRatios)
    {
        List<string> tickers = cedearsRatios.Select(x => x.Key).ToList();

        List<FinancialAsset> financialAssets = await GetFinancialAssetsDictionary(context, tickers);

        List<Cedear> cedears = financialAssets
            .Select(a => Cedear.Create(Guid.NewGuid(), a.Id, cedearsRatios[a.Ticker]))
            .ToList();

        context.Cedears.AddRange(cedears);
        await context.SaveChangesAsync();
    }

    private static async Task<List<FinancialAsset>> GetFinancialAssetsDictionary(ApplicationDbContext context, IEnumerable<string> tickers)
    {
        return await context
            .FinancialAssets
            .Where(a => tickers.Contains(a.Ticker))
            .ToListAsync();
    }
}
