using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.PriceEvaluator;

public class PriceEvaluatorService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IPriceMeasureRepository _measureRepository;
    private readonly IAlertService _alertService;

    public PriceEvaluatorService(
        IAlertRepository alertRepository, 
        IPriceMeasureRepository measureRepository, 
        IAlertService alertService)
    {
        _alertRepository = alertRepository;
        _measureRepository = measureRepository;
        _alertService = alertService;
    }

    public async Task EvaluatePriceMeasure(int priceMeasureId)
    {
        var priceMeasure = await _measureRepository.GetById(priceMeasureId);
        var pendingAlerts = await _alertRepository.GetAllWithPendingStateAndByFinancialAssetId(priceMeasure.FinancialAssetId);

        foreach (var alert in pendingAlerts)
        {
            if (PriceTargetEvaluator.IsTargetReached(alert, priceMeasure))
            {
                await _alertService.TriggerAlert(alert);
            }
        }
    }
}