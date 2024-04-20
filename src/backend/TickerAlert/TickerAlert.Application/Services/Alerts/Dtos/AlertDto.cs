namespace TickerAlert.Application.Services.Alerts.Dtos;

public class AlertDto
{
    public int FinancialAssetId { get; set; }
    public string TickerName { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public decimal Difference { get; set; }
    public string State { get; set; }
}