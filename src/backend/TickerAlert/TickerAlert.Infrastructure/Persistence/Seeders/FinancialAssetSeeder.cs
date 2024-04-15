using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Infrastructure.Persistence.Seeders;
/// <summary>
/// Seeder for filling stocks available data for NASDAQ
/// </summary>
public class FinancialAssetSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private const string SeedUrl = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token={API_KEY}";
    
    public FinancialAssetSeeder(ApplicationDbContext context, IHttpClientFactory factory, IConfiguration configuration)
    {
        _context = context;
        _httpClient = factory.CreateClient();
        _configuration = configuration;
    }
    
    public async Task Seed()
    {
        string apiKey = _configuration["Services:Finnhub:ApiKey"];
        var queryUrl = SeedUrl.Replace("{API_KEY}", apiKey);

        var financialAssetDtos = await _httpClient.GetFromJsonAsync<IEnumerable<FinancialAssetDto>>(queryUrl);

        Console.WriteLine(financialAssetDtos.Count());

        var only50 = financialAssetDtos.Take(50);
        
        Console.WriteLine(only50);
    }
}