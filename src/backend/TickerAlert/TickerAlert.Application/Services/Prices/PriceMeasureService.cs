using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.NotificationService;
using TickerAlert.Application.Interfaces.NotificationService.Dtos;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Prices;

public class PriceMeasureService(
    IApplicationDbContext context, 
    ILastPriceCacheService lastPriceCacheService,
    INotificationService notificationService) : IPriceMeasureService
{
    public async Task ProcessPriceMeasure(PriceMeasure measure)
    {
        await SavePriceMeasure(measure);
        await lastPriceCacheService.UpdatePriceAsync(measure.FinancialAssetId, measure.Price);
        await notificationService.BroadcastAssetPriceUpdate(new AssetPriceUpdateDto(measure.FinancialAssetId, measure.Price));
    }

    private async Task SavePriceMeasure(PriceMeasure measure)
    {
        measure.RaiseDomainEvent(new PriceReadedDomainEvent(Guid.NewGuid(), measure.Id));
        context.PriceMeasures.Add(measure);
        await context.SaveChangesAsync();
    }
}