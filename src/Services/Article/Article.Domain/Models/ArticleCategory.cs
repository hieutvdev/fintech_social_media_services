using Article.Domain.ValueObjects;

namespace Article.Domain.Models;

public class ArticleCategory
{
    public ArticleId ArticleId { get; private set; } = default!;

    public CategoryId CategoryId { get; private set; } = default!;

    public static ArticleCategory Create(ArticleId articleId, CategoryId categoryId)
    {
        var articleCategory = new ArticleCategory
        {
            ArticleId = articleId,
            CategoryId = categoryId
        };

        return articleCategory;
    }
}