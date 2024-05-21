using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface ISystemAlertReader
{
    Task<IEnumerable<Alert>> GetAllWithPendingStateAndByFinancialAssetId(Guid financialAssetId);
}