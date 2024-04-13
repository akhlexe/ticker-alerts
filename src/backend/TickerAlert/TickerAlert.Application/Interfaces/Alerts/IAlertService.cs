using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.Interfaces.Alerts
{
    public interface IAlertService
    {
        Task CreateAlert(int financialAssetId, decimal targetPrice);
    }
}
