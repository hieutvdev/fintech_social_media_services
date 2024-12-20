namespace Article.Application.DTOs.Request.Article;

public record UpdateArticleRequestDto(
    string Id,
    string Title,
    string Description,
    string Content,
    string MainImageUrl,
    string[]? Tags,
    string[]? Categories);
    
    