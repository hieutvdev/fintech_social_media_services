
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Persistence.Data.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasConversion(userInfoId => userInfoId.Value, dbId => UserInfoId.Of(dbId));

        builder.Property(r => r.BirthDate).HasDefaultValue(0);
        builder.HasIndex(r => r.FullName);
        builder.Property(r => r.FullName).IsRequired();
        
        builder.HasIndex(r => r.UserId).IsUnique();
        
        builder.Property(r => r.UserId).IsRequired();
        
        
    }
}