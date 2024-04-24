using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> entity)
    {
        entity.HasKey(a => a.Id);

        entity.Property(a => a.FinancialAssetId)
            .IsRequired();

        entity.Property(a => a.UserId)
            .IsRequired();
        
        entity.Property(a => a.TargetPrice)
            .IsRequired()
            .HasColumnType("decimal(15, 2)");

        entity.Property(a => a.ThresholdType)
            .IsRequired();

        entity.Property(a => a.State)
            .IsRequired();

        entity.HasOne(a => a.FinancialAsset)
            .WithMany()
            .HasForeignKey(a => a.FinancialAssetId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(a => a.FinancialAssetId);

        entity.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}