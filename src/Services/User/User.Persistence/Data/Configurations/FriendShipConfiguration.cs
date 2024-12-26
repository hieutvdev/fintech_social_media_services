using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Persistence.Data.Configurations;

public class FriendShipConfiguration : IEntityTypeConfiguration<FriendShip>
{
    public void Configure(EntityTypeBuilder<FriendShip> builder)
    {
        throw new NotImplementedException();
    }
}