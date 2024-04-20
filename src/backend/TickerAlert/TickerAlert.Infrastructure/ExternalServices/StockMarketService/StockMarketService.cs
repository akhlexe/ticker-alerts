using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService.Dtos;

namespace TickerAlert.Infrastructure.ExternalServices.StockMarketService;

public class StockMarketService : IStockMarketService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private string _baseUri = "https://finnhub.io/api/v1";

    public StockMarketService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<PriceMeasureDto> ReadPriceFor(string ticker)
    {
        var apiKey = _configuration["Services:Finnhub:ApiKey"];

        var path = "/quote?symbol={TICKER}&token={ApiKey}"
            .Replace("{TICKER}", ticker)
            .Replace("{ApiKey}", apiKey);

        var query = _baseUri + path;

        var priceRead = await _httpClient.GetFromJsonAsync<PriceMeasureReadDto>(query);
        return new PriceMeasureDto() { CurrentPrice = priceRead?.CurrentPrice ?? 0 };
    }
}