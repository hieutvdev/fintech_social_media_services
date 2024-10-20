using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
            orderId => orderId.Value, dbId => CategoryId.Of(dbId));

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Slug)
            .IsUnicode();

        builder.HasMany(r => r.ArticleCategories)
            .WithOne()
            .HasForeignKey(r => r.CategoryId);


    }
}