using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.HasKey(i => new { i.ArticleId, i.CategoryId });

        builder.Property(i => i.ArticleId).HasConversion(articleId => articleId.Value, dbId => ArticleId.Of(dbId));
        builder.Property(i => i.CategoryId).HasConversion(categoryId => categoryId.Value, dbId => CategoryId.Of(dbId));
        
    }
}