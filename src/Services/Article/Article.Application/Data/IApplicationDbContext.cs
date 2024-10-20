using Article.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Article.Application.Data;

public interface IApplicationDbContext
{
    DbSet<ArticleTag> ArticleTags { get; }
    
    DbSet<Article.Domain.Models.Article> Articles { get; }
    
    DbSet<Category> Categories { get; }
    
    DbSet<ArticleCategory> ArticleCategories { get; }
    
    DbSet<Tag> Tags { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}