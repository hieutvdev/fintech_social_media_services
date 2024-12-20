using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("tbl.Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(150);
        builder.Property(x => x.AvatarUrl).HasDefaultValue("").IsRequired(false);
        builder.Property(x => x.Status).HasDefaultValue(1);
        builder.HasMany<UserSession>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
    }
}