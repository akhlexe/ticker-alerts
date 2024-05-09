using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public class FinancialAssetConfiguration : IEntityTypeConfiguration<FinancialAsset>
{
    public void Configure(EntityTypeBuilder<FinancialAsset> entity)
    {
        entity.HasKey(f => f.Id)
            .IsClustered(false);
        
        entity.Property(f => f.Name)
            .IsRequired();
        
        entity.Property(f => f.Ticker)
            .IsRequired();
    }
}