using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertRepository
{
    // User interaction scope
    Task CreateAlert(Guid userId, Guid financialAssetId, decimal targetPrice, PriceThresholdType thresholdType);
    Task<IEnumerable<Alert>> GetAllForUserId(Guid userId);
    
    // Background process access
    Task<IEnumerable<Alert>> GetAllWithPendingStateAndByFinancialAssetId(Guid financialAssetId);
    Task UpdateRange(IEnumerable<Alert> alerts);
    
    // TODO: Modificar esto luego de implementar UoW
    Task TriggerAlert(Alert alert);
}