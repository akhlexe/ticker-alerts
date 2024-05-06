using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertRepository
{
    // User interaction scope
    Task CreateAlert(int userId, int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType);
    Task<IEnumerable<Alert>> GetAllForUserId(int userId);
    
    // Background process access
    Task<IEnumerable<Alert>> GetAllWithPendingStateAndByFinancialAssetId(int financialAssetId);
    Task UpdateRange(IEnumerable<Alert> alerts);
    
    // TODO: Modificar esto luego de implementar UoW
    Task TriggerAlert(Alert alert);
}