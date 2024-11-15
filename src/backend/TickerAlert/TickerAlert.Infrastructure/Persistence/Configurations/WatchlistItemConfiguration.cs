using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

internal sealed class WatchlistItemConfiguration : IEntityTypeConfiguration<WatchlistItem>
{
    public void Configure(EntityTypeBuilder<WatchlistItem> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FinancialAssetId)
            .IsRequired();
    }
}
