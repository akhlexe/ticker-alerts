using MediatR;
using Microsoft.Extensions.Logging;
using TickerAlert.Application.Services.PriceEvaluator;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.PriceReader;

public class PriceReadedEventHandler : INotificationHandler<PriceReadedDomainEvent>
{
    private readonly PriceEvaluatorService _priceEvaluatorService;
    private readonly ILogger<PriceReadedEventHandler> _logger;

    public PriceReadedEventHandler(
        PriceEvaluatorService priceEvaluatorService, 
        ILogger<PriceReadedEventHandler> logger)
    {
        _priceEvaluatorService = priceEvaluatorService;
        _logger = logger;
    }

    public async Task Handle(PriceReadedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting handling price measure (Id = {notification.PriceMeasureId})");
        
        await _priceEvaluatorService.EvaluatePriceMeasure(notification.PriceMeasureId);
        
        _logger.LogInformation($"Finishing handling price measure (Id = {notification.PriceMeasureId})");
    }
}