using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.PriceEvaluator;

public class PriceEvaluatorService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IPriceMeasureRepository _measureRepository;

    public PriceEvaluatorService(IAlertRepository alertRepository, IPriceMeasureRepository measureRepository)
    {
        _alertRepository = alertRepository;
        _measureRepository = measureRepository;
    }

    public async Task EvaluatePriceMeasure(int priceMeasureId)
    {
        var priceMeasure = await _measureRepository.GetById(priceMeasureId);
        var pendingAlerts = await _alertRepository.GetAllWithPendingStateAndByFinancialAssetId(priceMeasure.FinancialAssetId);

        var updatedAlerts = pendingAlerts
            .Select(alert => EvaluateAndUpdateAlert(alert, priceMeasure))
            .ToList();
        
        await _alertRepository.UpdateRange(updatedAlerts);
    }

    private static Alert EvaluateAndUpdateAlert(Alert alert, PriceMeasure priceMeasure)
    {
        if (PriceTargetEvaluator.IsTargetReached(alert, priceMeasure))
        {
            alert.State = AlertState.TRIGGERED;
        }

        return alert;
    }
}