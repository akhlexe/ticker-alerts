using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.Alerts
{
    public interface IAlertService
    {
        Task<Guid> CreateAlert(Guid financialAssetId, decimal targetPrice);
        Task TriggerAlert(Alert alert);
        Task NotifyAlert(Alert alert);
    }
}
