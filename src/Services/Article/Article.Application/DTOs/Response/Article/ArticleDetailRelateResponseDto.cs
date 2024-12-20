namespace Article.Application.DTOs.Response.Article;

public record ArticleDetailRelateResponseDto(ArticleGetDetailsResponseDto Article, IEnumerable<ArticleRelateResponseDto> Relates, IEnumerable<ArticleRelateResponseDto> Hots);