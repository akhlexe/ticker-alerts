using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertsReader
{
    Task<IEnumerable<Alert>> GetAlerts();
}