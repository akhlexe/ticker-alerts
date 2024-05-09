namespace TickerAlert.Application.Services.FinancialAssets.Dtos;

public class FinancialAssetDto
{
    public Guid Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }
}