using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Events;
using TickerAlert.Infrastructure.Persistence.Outbox;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class PriceMeasureRepository : IPriceMeasureRepository
{
    private readonly ApplicationDbContext _context;

    public PriceMeasureRepository(ApplicationDbContext context) => _context = context;

    public async Task<PriceMeasure?> GetLastPriceMeasureFor(int financialAssetId)
    {
        return await _context.PriceMeasures
            .Where(p => p.FinancialAssetId == financialAssetId)
            .OrderByDescending(p => p.MeasuredOn)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PriceMeasure?>> GetLastPricesMeasuresFor(IEnumerable<int> financialAssetsIds)
    {
        return await _context.PriceMeasures
            .Where(x => financialAssetsIds.Contains(x.FinancialAssetId))
            .GroupBy(x => x.FinancialAssetId)
            .Select(g => g.OrderByDescending(r => r.MeasuredOn).FirstOrDefault())
            .ToListAsync();
    }

    public async Task RegisterPriceMeasure(PriceMeasure measure)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _context.PriceMeasures.Add(measure);
            await _context.SaveChangesAsync();
        
            var priceReadedDomainEvent = new PriceReadedDomainEvent(Guid.NewGuid(), measure.Id);
            _context.OutboxMessages.Add(OutboxMessageBuilder.CreateOutboxMessage(priceReadedDomainEvent));    
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<PriceMeasure?> GetById(int id)
    {
        return await _context.PriceMeasures.FirstOrDefaultAsync(p => p.Id == id);
    }
}