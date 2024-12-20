namespace Article.Application.DTOs.Response.Article;

public record ArticleResponseBaseDto(
    string Id,
    string Title,
    string Description,
    string MainImageUrl,
    string Slug,
    DateTime? PublishAt,
    string Tags,
    string CategoryName);
