using Microsoft.EntityFrameworkCore;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Common.Persistence;

public interface IApplicationDbContext
{
    public DbSet<Alert> Alerts { get; }
    public DbSet<PriceMeasure> PriceMeasures { get; }
    public DbSet<FinancialAsset> FinancialAssets { get; }
    public DbSet<Watchlist> Watchlists { get; }
    public DbSet<WatchlistItem> WatchlistItems { get; }

    Task<int> SaveChangesAsync();
}