using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertRepository
{
    // User interaction scope
    Task<Alert?> GetById(Guid alertId);
    Task<IEnumerable<Alert>> GetAllForUserId(Guid userId);
    
    // Background process access
    Task<IEnumerable<Alert>> GetAllWithPendingStateAndByFinancialAssetId(Guid financialAssetId);
    // Task UpdateRange(IEnumerable<Alert> alerts);
    
    // TODO: Modificar esto luego de implementar UoW
    Task TriggerAlert(Alert alert);
    Task NotifyAlert(Alert alert);
}