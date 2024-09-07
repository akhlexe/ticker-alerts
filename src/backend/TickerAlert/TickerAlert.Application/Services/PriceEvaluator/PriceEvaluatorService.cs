using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Services;

namespace TickerAlert.Application.Services.PriceEvaluator;

public class PriceEvaluatorService(
    ISystemAlertReader systemAlertReader,
    IPriceMeasureReader measureReader,
    IAlertService alertService)
{
    public async Task EvaluatePriceMeasure(Guid priceMeasureId)
    {
        var priceMeasure = await measureReader.GetById(priceMeasureId);
        var pendingAlerts = await systemAlertReader.GetPendingAlertsByFinancialAsset(priceMeasure.FinancialAssetId);

        foreach (var alert in pendingAlerts)
        {
            if (PriceTargetEvaluator.IsTargetReached(alert, priceMeasure))
            {
                await alertService.TriggerAlert(alert);
            }
        }
    }
}