using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Alerts;

public class AlertReader : IAlertReader
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAlertRepository _repository;
    private readonly IPriceMeasureRepository _priceMeasureRepository;
    
    public AlertReader(
        IAlertRepository repository, 
        IPriceMeasureRepository priceMeasureRepository, 
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _priceMeasureRepository = priceMeasureRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        var alerts = await _repository.GetAllForUserId(_currentUserService.UserId);
        var lastPrices = await _priceMeasureRepository.GetLastPricesMeasuresFor(alerts.Select(x => x.FinancialAssetId));
        return alerts.Select(alert => CreateAlertDto(alert, lastPrices.FirstOrDefault(p => p.FinancialAssetId == alert.FinancialAssetId)));
    }

    private static AlertDto CreateAlertDto(Alert a, PriceMeasure? priceMeasure)
    {
        var actualPrice = priceMeasure?.Price ?? 0;

        return new AlertDto()
        {
            FinancialAssetId = a.FinancialAssetId,
            TickerName = a.FinancialAsset.Ticker,
            TargetPrice = a.TargetPrice,
            ActualPrice = actualPrice,
            Difference = a.TargetPrice - actualPrice,
            State = a.State.ToString()
        };
    }
}