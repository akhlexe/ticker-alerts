using Microsoft.EntityFrameworkCore;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence.Configurations;

namespace TickerAlert.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Alert> Alerts { get; set; }
    public DbSet<FinancialAsset> FinancialAssets { get; set; }
    public DbSet<PriceMeasure> PriceMeasures { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}