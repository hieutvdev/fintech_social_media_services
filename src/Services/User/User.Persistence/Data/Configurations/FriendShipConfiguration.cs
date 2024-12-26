using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class FriendShipConfiguration : IEntityTypeConfiguration<FriendShip>
{
    public void Configure(EntityTypeBuilder<FriendShip> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(friendShipId => friendShipId.Value, dbId => FriendShipId.Of(dbId));
        
        builder.HasIndex(r => r.FriendId);

        builder.HasIndex(r => r.UserId);
        
        builder.Property(r => r.FriendId).IsRequired();
        
        builder.Property(r => r.UserId).IsRequired();
    }
}