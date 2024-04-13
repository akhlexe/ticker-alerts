using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.Alerts;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _repository;

    public AlertService(IAlertRepository repository) => _repository = repository;

    public async Task CreateAlert(int financialAssetId, decimal targetPrice)
    {
        decimal currentPrice = await GetCurrentPrice(financialAssetId);
        PriceThresholdType threshold = ThresholdResolver.Resolve(currentPrice, targetPrice);
        await _repository.CreateAlert(financialAssetId, targetPrice, threshold);
    }

    // TODO: Definir servicio externo para consumir precios de activos financieros
    private async Task<decimal> GetCurrentPrice(int financialAssetId)
    {
        return 0;
    }
}