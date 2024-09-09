using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Infrastructure.ExternalServices.StockMarketService.Dtos;

namespace TickerAlert.Infrastructure.ExternalServices.StockMarketService;

public class StockMarketService : IStockMarketService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public StockMarketService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _baseUrl = configuration["Services:Finnhub:BaseUrl"];
        _apiKey = configuration["Services:Finnhub:ApiKey"];
        
        if (string.IsNullOrEmpty(_baseUrl))
        {
            throw new ArgumentNullException(nameof(_baseUrl), "Base URL configuration is missing in the settings.");
        }
    
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new ArgumentNullException(nameof(_apiKey), "API Key configuration is missing in the settings.");
        }
        
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<PriceMeasureDto> ReadPriceFor(string ticker)
    {
        var query = _baseUrl + GetQuotePath(ticker);
        var priceRead = await _httpClient.GetFromJsonAsync<PriceMeasureReadDto>(query);
        return new PriceMeasureDto() { CurrentPrice = priceRead?.CurrentPrice ?? 0 };
    }

    private string GetQuotePath(string ticker)
    {
        return $"/quote?symbol={ticker}&token={_apiKey}";
    }

    public async Task<CompanyProfileDto> GetCompanyProfile(string ticker)
    {
        var query = _baseUrl + $"/stock/profile2?symbol={ticker}&token={_apiKey}";
        CompanyProfileApiDto? companyProfileApiDto = await _httpClient.GetFromJsonAsync<CompanyProfileApiDto>(query);

        return companyProfileApiDto?.Ticker is null
            ? CompanyProfileMapper.CreateNullCompanyProfile(ticker)
            : CompanyProfileMapper.ToCompanyProfileDto(companyProfileApiDto);
    }
}