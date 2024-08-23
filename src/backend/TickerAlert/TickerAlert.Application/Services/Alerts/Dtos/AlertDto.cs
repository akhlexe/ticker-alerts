using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Alerts.Dtos;

public class AlertDto
{
    public Guid Id { get; set; }
    public Guid FinancialAssetId { get; set; }
    public string TickerName { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public decimal Difference { get; set; }
    public AlertState State { get; set; }
}