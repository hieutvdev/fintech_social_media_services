using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.HasKey(at => new { at.ArticleId, at.TagId });
        builder.Property(i => i.ArticleId).HasConversion(articleId => articleId.Value, dbId => ArticleId.Of(dbId));
        builder.Property(i => i.TagId).HasConversion(tagId => tagId.Value, dbId => TagId.Of(dbId));
    }
}