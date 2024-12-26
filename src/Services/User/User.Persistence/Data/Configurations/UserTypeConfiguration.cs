using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Persistence.Data.Configurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired();

        builder.HasMany(r => r.UserInfos)
            .WithOne()
            .HasForeignKey(r => r.UserType)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}