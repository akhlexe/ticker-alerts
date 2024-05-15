using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.Alerts;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _repository;
    private readonly IPriceMeasureRepository _priceMeasureRepository;
    private readonly ICurrentUserService _currentUserService;

    public AlertService(
        IAlertRepository repository, 
        IPriceMeasureRepository priceMeasureRepository, 
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _priceMeasureRepository = priceMeasureRepository;
        _currentUserService = currentUserService;
    }

    public async Task CreateAlert(Guid financialAssetId, decimal targetPrice)
    {
        decimal currentPrice = await GetCurrentPrice(financialAssetId);
        PriceThresholdType threshold = ThresholdResolver.Resolve(currentPrice, targetPrice);
        
        await _repository.CreateAlert(
            _currentUserService.UserId, 
            financialAssetId, 
            targetPrice, 
            threshold
        );
    }

    public async Task TriggerAlert(Alert alert)
    {
        await _repository.TriggerAlert(alert);
    }

    private async Task<decimal> GetCurrentPrice(Guid financialAssetId)
    {
        var priceMeasure = await _priceMeasureRepository.GetLastPriceMeasureFor(financialAssetId);
        return priceMeasure?.Price ?? 0;
    }
}