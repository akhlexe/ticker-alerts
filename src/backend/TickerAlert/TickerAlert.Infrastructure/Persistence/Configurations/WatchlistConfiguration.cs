using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public sealed class WatchlistConfiguration : IEntityTypeConfiguration<Watchlist>
{
    public void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();
    }
}
