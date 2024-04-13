using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Alerts;

public class AlertReader : IAlertReader
{
    private readonly IAlertRepository _repository;
    private readonly IPriceMeasureRepository _priceMeasureRepository;
    
    public AlertReader(IAlertRepository repository, IPriceMeasureRepository priceMeasureRepository)
    {
        _repository = repository;
        _priceMeasureRepository = priceMeasureRepository;
    }

    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        var alerts = await _repository.GetAll();
        var assetsIds = alerts.Select(x => x.FinancialAssetId).ToList();
        var lastPrices = await _priceMeasureRepository.GetLastPricesMeasuresFor(assetsIds);
        return alerts.Select(a => CreateAlertDto(a, lastPrices)).ToList();
    }

    private static AlertDto CreateAlertDto(Alert a, IEnumerable<PriceMeasure> lastPrices)
    {
        var lastMeasure = lastPrices.FirstOrDefault(p=> p.FinancialAssetId == a.FinancialAssetId);
        var lastPrice = lastMeasure?.Price ?? 0;

        return new AlertDto()
        {
            TickerName = a.FinancialAsset.Ticker,
            TargetPrice = a.TargetPrice,
            ActualPrice = lastPrice,
            Difference = a.TargetPrice - lastMeasure?.Price ?? 0,
            State = "PENDING"
        };
    }
}