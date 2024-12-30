using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public class CedearConfiguration : IEntityTypeConfiguration<Cedear>
{
    public void Configure(EntityTypeBuilder<Cedear> entity)
    {
        entity.HasKey(c => c.Id);

        entity.Property(c => c.FinancialAssetId)
            .IsRequired();

        entity.Property(c => c.Ratio)
            .IsRequired();
    }
}
