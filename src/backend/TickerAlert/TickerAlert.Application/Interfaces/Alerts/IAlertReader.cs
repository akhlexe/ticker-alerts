using TickerAlert.Application.Services.Alerts.Dtos;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertReader
{
    Task<IEnumerable<AlertDto>> GetAlerts();
}