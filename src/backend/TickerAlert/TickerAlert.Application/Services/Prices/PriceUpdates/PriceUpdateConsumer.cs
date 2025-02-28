using Microsoft.Extensions.Logging;
using TickerAlert.Application.Common.EventBus;
using TickerAlert.Application.Services.PriceEvaluator;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Prices.PriceUpdates;

public class PriceUpdateConsumer(
    PriceEvaluatorService priceEvaluatorService,
    ILogger<PriceUpdateConsumer> logger) : IEventConsumer<PriceUpdateEvent>
{
    public async Task HandleAsync(PriceUpdateEvent eventMessage, CancellationToken cancellationToken)
    {
        await priceEvaluatorService.EvaluatePriceMeasure(eventMessage.PriceMeasureId);

        logger.LogInformation("Price update evaluated (Id = {PriceMeasureId})", eventMessage.PriceMeasureId);
    }
}
