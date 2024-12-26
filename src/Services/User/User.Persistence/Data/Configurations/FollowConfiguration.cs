using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(followId => followId.Value, dbId => FollowId.Of(dbId));
        builder.HasIndex(r => r.FollowerId);
        
        builder.HasIndex(r => r.FollowingId);
        
        builder.Property(r => r.FollowerId).IsRequired();
        
        builder.Property(r => r.FollowingId).IsRequired();
    }
}