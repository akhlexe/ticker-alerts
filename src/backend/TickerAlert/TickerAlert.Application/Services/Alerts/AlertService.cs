using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.Alerts;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _repository;
    private readonly IPriceMeasureRepository _priceMeasureRepository;

    public AlertService(IAlertRepository repository, IPriceMeasureRepository priceMeasureRepository)
    {
        _repository = repository;
        _priceMeasureRepository = priceMeasureRepository;
    }

    public async Task CreateAlert(int financialAssetId, decimal targetPrice)
    {
        decimal currentPrice = await GetCurrentPrice(financialAssetId);
        PriceThresholdType threshold = ThresholdResolver.Resolve(currentPrice, targetPrice);
        await _repository.CreateAlert(financialAssetId, targetPrice, threshold);
    }

    private async Task<decimal> GetCurrentPrice(int financialAssetId)
    {
        var priceMeasure = await _priceMeasureRepository.GetLastPriceMeasureFor(financialAssetId);
        return priceMeasure.Price;
    }
}