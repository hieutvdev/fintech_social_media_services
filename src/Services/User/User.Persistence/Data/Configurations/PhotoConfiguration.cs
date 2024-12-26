using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasConversion(photoId => photoId.Value, dbId => PhotoId.Of(dbId));

        builder.HasIndex(r => r.UserId);

        builder.Property(r => r.UserId).IsRequired();
        
        builder.Property(r => r.Url).IsRequired();
    }
}