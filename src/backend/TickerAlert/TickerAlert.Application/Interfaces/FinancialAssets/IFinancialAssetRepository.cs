using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.FinancialAssets;

public interface IFinancialAssetRepository
{
    Task<IEnumerable<FinancialAsset>> GetAllBySearchCriteria(string criteria);
    Task<IEnumerable<FinancialAsset>> GetAllWithPendingAlerts();
}