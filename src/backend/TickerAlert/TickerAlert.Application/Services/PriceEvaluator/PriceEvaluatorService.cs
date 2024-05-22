using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.PriceEvaluator;

public class PriceEvaluatorService
{
    private readonly ISystemAlertReader _systemAlertReader;
    private readonly IPriceMeasureReader _measureReader;
    private readonly IAlertService _alertService;

    public PriceEvaluatorService(
        ISystemAlertReader systemAlertReader, 
        IPriceMeasureReader measureReader, 
        IAlertService alertService)
    {
        _systemAlertReader = systemAlertReader;
        _measureReader = measureReader;
        _alertService = alertService;
    }

    public async Task EvaluatePriceMeasure(Guid priceMeasureId)
    {
        var priceMeasure = await _measureReader.GetById(priceMeasureId);
        var pendingAlerts = await _systemAlertReader.GetPendingAlertsByFinancialAsset(priceMeasure.FinancialAssetId);

        foreach (var alert in pendingAlerts)
        {
            if (PriceTargetEvaluator.IsTargetReached(alert, priceMeasure))
            {
                await _alertService.TriggerAlert(alert);
            }
        }
    }
}