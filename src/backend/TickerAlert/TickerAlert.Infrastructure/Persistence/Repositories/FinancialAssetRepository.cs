using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Repositories;

public class FinancialAssetRepository : IFinancialAssetRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialAssetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialAsset>> GetAllBySearchCriteria(string criteria)
    {
        return await _context.FinancialAssets
            .Where(x => x.Name.Contains(criteria) || x.Ticker.Contains(criteria))
            .ToListAsync();
    }
}