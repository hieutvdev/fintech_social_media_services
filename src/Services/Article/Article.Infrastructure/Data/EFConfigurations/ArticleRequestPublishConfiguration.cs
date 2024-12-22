using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ArticleRequestPublishConfiguration : IEntityTypeConfiguration<ArticleRequestPublish>
{
    public void Configure(EntityTypeBuilder<ArticleRequestPublish> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasConversion(
            arp => arp.Value, dbId => ArticleRequestPublishId.Of(dbId));

        builder.Property(a => a.ArticleId).HasConversion(
            articleId => articleId.Value, dbId => ArticleId.Of(dbId));
        
        
        builder.HasMany(a => a.ProcessingSteps)
            .WithOne()
            .HasForeignKey(a => a.ArticleRequestPublishId)
            .OnDelete(DeleteBehavior.Cascade);
        
        

    }
}