namespace TickerAlert.Application.UseCases.Alerts.GetAlerts.Dtos;

public class AlertDto
{
    public string TickerName { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public decimal Difference { get; set; }
    public string State { get; set; }
}