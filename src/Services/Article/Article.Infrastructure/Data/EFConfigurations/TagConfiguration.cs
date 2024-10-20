using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasConversion(tagId => tagId.Value, dbId => TagId.Of(dbId));
        
        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Slug)
            .IsUnicode()
            .IsRequired();

    }
}