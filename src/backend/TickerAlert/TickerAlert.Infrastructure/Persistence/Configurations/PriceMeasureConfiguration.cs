using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public class PriceMeasureConfiguration : IEntityTypeConfiguration<PriceMeasure>
{
    public void Configure(EntityTypeBuilder<PriceMeasure> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.FinancialAssetId)
            .IsRequired();

        entity.Property(e => e.Price)
            .IsRequired()
            .HasColumnType("decimal(15, 2)");

        entity.Property(e => e.MeasuredOn)
            .IsRequired();

        entity.HasOne(e => e.FinancialAsset)
            .WithMany()
            .HasForeignKey(e => e.FinancialAssetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}