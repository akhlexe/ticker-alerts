using Microsoft.EntityFrameworkCore;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public Alert Alerts { get; set; }
    public FinancialAsset FinancialAssets { get; set; }
    public PriceMeasure PriceMeasures { get; set; }
}