using Auth.Domain.Enums;
using Auth.Domain.Models;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.EntityConfigurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("tbl.UserSessions");
        builder.HasKey(x => x.Id);
         builder.HasOne(x => x.User)
                    .WithMany(x => x.UserSessions)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();
        builder.Property(x => x.Id).HasConversion(
            userSessionId => userSessionId.Value,
            dbId => UserSessionId.Of(dbId));
        builder.Property(x => x.PushToken).IsRequired();
    }
}