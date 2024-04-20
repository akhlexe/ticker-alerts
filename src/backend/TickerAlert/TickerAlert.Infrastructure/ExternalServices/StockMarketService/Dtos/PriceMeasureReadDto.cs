using System.Text.Json.Serialization;

namespace TickerAlert.Infrastructure.ExternalServices.StockMarketService.Dtos;

public class PriceMeasureReadDto
{
    [JsonPropertyName("c")]
    public decimal CurrentPrice { get; set; }
}