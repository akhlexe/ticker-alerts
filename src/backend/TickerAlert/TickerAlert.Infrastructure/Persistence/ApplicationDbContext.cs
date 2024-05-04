using Microsoft.EntityFrameworkCore;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence.Outbox;

namespace TickerAlert.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<Alert> Alerts { get; set; }
    public DbSet<FinancialAsset> FinancialAssets { get; set; }
    public DbSet<PriceMeasure> PriceMeasures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}