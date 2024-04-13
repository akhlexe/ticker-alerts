using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Interfaces.Alerts;

public interface IAlertRepository
{
    Task CreateAlert(int financialAssetId, decimal targetPrice, PriceThresholdType thresholdType);
    Task<IEnumerable<Alert>> GetAll();
}