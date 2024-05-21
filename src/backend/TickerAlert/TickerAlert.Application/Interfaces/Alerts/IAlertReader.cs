using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertReader
{
    Task<IEnumerable<AlertDto>> GetAlerts();
    Task<Alert?> GetById(Guid alertId);
    Task<IEnumerable<Alert>> GetAllForUserId(Guid userId);
}