using Article.Domain.ValueObjects;

namespace Article.Domain.Models;

public class ArticleTag
{
    public ArticleId ArticleId { get; private set; } = default!;
    
    public TagId TagId { get; private set; } = default!;


    public static ArticleTag Create(ArticleId articleId, TagId tagId)
    {
        var articleTag = new ArticleTag
        {
            ArticleId = articleId,
            TagId = tagId
        };

        return articleTag;
    }
}