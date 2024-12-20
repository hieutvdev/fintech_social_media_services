namespace Article.Application.DTOs.Response.Article;

public record ArticleByAdminResponseDto(
    string Id,
    string Title,
    string Description,
    string MainImageUrl,
    string CreatedBy,
    int Status,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    int Type
    );
