using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Username).IsRequired();
        entity.Property(e => e.HashedPassword).IsRequired();
        entity.Property(e => e.CreatedOn).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        
        entity.HasIndex(e => e.Username).IsUnique();
    }
}