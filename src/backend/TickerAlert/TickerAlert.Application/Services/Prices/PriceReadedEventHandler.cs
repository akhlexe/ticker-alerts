using MediatR;
using Microsoft.Extensions.Logging;
using TickerAlert.Application.Services.PriceEvaluator;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Prices;

public class PriceReadedEventHandler(
    PriceEvaluatorService priceEvaluatorService,
    PriceReadedNotifier priceReadedNotifier,
    ILogger<PriceReadedEventHandler> logger) : INotificationHandler<PriceReadedDomainEvent>
{
    public async Task Handle(PriceReadedDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await priceReadedNotifier.NotifyPriceReaded(notification.PriceMeasureId);
            await priceEvaluatorService.EvaluatePriceMeasure(notification.PriceMeasureId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Error evaluating price for PriceMeasure = {FinancialAsset}. Error message = {ErrorMessage}", 
                notification.PriceMeasureId, 
                ex.Message);
        }
        
    }
}