namespace Article.Application.DTOs.Request.Article;

public record CreateArticleRequestDto(
    string Title,
    string Description,
    string Content,
    string MainImageUrl,
    string[]? Tags,
    string[]? Categories
    );
 