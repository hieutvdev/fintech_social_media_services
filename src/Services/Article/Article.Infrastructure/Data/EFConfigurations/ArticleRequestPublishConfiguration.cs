using Article.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ArticleRequestPublishConfiguration : IEntityTypeConfiguration<ArticleRequestPublish>
{
    public void Configure(EntityTypeBuilder<ArticleRequestPublish> builder)
    {
        throw new NotImplementedException();
    }
}