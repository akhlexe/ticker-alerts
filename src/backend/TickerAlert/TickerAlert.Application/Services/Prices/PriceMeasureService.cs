using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Events;

namespace TickerAlert.Application.Services.Prices;

public class PriceMeasureService : IPriceMeasureService
{
    private readonly IApplicationDbContext _context;

    public PriceMeasureService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task RegisterPriceMeasure(PriceMeasure measure)
    {
        measure.RaiseDomainEvent(new PriceReadedDomainEvent(Guid.NewGuid(), measure.Id));
        _context.PriceMeasures.Add(measure);
        await _context.SaveChangesAsync();
    }
}