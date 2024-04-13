using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Services.Alerts;

public class AlertsReader : IAlertsReader
{
    public Task<IEnumerable<Alert>> GetAlerts()
    {
        return Task.FromResult<IEnumerable<Alert>>(new List<Alert>()
        {
            new Alert(1, 1000, PriceThresholdType.Above),
            new Alert(2, 500, PriceThresholdType.Below),
        });
    }
}