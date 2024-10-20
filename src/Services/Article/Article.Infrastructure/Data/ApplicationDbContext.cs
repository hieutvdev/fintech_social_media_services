
using System.Reflection;
using Article.Application.Data;
using Article.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();

    public DbSet<Article.Domain.Models.Article> Articles => Set<Article.Domain.Models.Article>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}