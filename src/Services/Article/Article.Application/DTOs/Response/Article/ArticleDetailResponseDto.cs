namespace Article.Application.DTOs.Response.Article;

public record ArticleDetailResponseDto(ArticleGetDetailsResponseDto Details, IEnumerable<ArticleResponseBaseDto> ArticleRelations);