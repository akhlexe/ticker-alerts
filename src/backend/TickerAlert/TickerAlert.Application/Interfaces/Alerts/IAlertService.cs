using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Alerts
{
    public interface IAlertService
    {
        Task CreateAlert(int financialAssetId, decimal targetPrice);
        Task TriggerAlert(Alert alert);
    }
}
