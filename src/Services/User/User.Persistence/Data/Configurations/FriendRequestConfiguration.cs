using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(friendRequestId => friendRequestId.Value, dbId => FriendRequestId.Of(dbId));
        
        builder.HasIndex(r => r.SenderId);

        builder.HasIndex(r => r.ReceiverId);
        
        builder.Property(r => r.Status)
            .HasConversion<int>();

        builder.Property(r => r.SendAt).HasDefaultValue(DateTime.UtcNow);
        
        builder.Property(r => r.ResponseAt).HasDefaultValue(null);

        builder.Property(r => r.ReceiverId).IsRequired();

        builder.Property(r => r.SenderId).IsRequired();



    }
}