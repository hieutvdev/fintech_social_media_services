namespace Article.Application.DTOs.Response.Article;

public record ArticleByUserResponseDto(
    string Id,
    string Title,
    string Description,
    string MainImageUrl,
    int Status,
    DateTime? CreatedAt,
    DateTime? UpdatedAt);
