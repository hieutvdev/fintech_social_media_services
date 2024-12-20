using Article.Domain.Enums;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article.Domain.Models.Article>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasConversion(
            articleId => articleId.Value, dbId => ArticleId.Of(dbId));

        builder.Property(a => a.Slug)
            .IsUnicode();

        builder.Property(a => a.AuthorId).HasConversion(
            authorId => authorId.Value, dbId => AuthorId.Of(dbId));

        builder.Property(a => a.Type).HasDefaultValue((ArticleType)1);
        

        builder.HasMany(a => a.ArticleCategories)
            .WithOne()
            .HasForeignKey(a => a.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.ArticleTags)
            .WithOne()
            .HasForeignKey(a => a.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}