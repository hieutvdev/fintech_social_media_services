using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasConversion(userInfoId => userInfoId.Value, dbId => UserInfoId.Of(dbId));

        builder.HasIndex(r => r.UserId).IsUnique();
        
        builder.Property(r => r.UserId).IsRequired();
        
        
    }
}