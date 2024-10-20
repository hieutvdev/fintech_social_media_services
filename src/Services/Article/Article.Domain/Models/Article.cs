
using Article.Domain.Enums;
using Article.Domain.ValueObjects;
using BuildingBlocks.Helpers;
using ShredKernel.Aggregates;

namespace Article.Domain.Models;

public class Article : Entity<ArticleId>
{
    private readonly List<ArticleTag> _articleTags = new();
    public IReadOnlyList<ArticleTag> ArticleTags => _articleTags.AsReadOnly();

    private readonly List<ArticleCategory> _articleCategories = new();
    public IReadOnlyList<ArticleCategory> ArticleCategories => _articleCategories.AsReadOnly();
    
    public string Title { get; private set; } = default!;
    
    public string Description { get; private set; } = default!;
    
    public string Content { get; private set; } = default!;
    
    public string MainImageUrl { get; private set; } = default!;
    
    public string Slug { get; private set; } = default!;
    
    public ArticleStatus Status { get; private set; } = default!;

    public AuthorId AuthorId { get; private set; } = default!;
    
    public DateTime? PublishAt { get; set; }


    public static Article Create(ArticleId articleId, string title, string description, string content,
        string mainImageUrl, AuthorId authorId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(mainImageUrl);

        var article = new Article
        {
            Id = articleId,
            Title = title,
            Description = description,
            Content = content,
            Slug = SlugHelper.GenerateSlug(title),
            MainImageUrl = mainImageUrl,
            Status = ArticleStatus.Draft,
            AuthorId = authorId
        };

        return article;
    }
    
   
}