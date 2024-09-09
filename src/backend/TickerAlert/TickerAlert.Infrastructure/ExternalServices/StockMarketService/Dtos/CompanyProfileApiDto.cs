using System.Text.Json.Serialization;

namespace TickerAlert.Infrastructure.ExternalServices.StockMarketService.Dtos;

public sealed class CompanyProfileApiDto
{
    //[JsonPropertyName("country")]
    public string Country { get; set; }
    public string Currency { get; set; }
    public string Exchange { get; set; }
    public DateTime Ipo { get; set; }
    public decimal MarketCapitalization { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public decimal ShareOutstanding { get; set; }
    public string Ticker { get; set; }
    public string Weburl { get; set; }
    public string Logo { get; set; }
    public string FinnhubIndustry { get; set; }
}
