namespace Article.Application.DTOs.Response.Article;

public record ArticleGetDetailsResponseDto(string Id, string Title, string Description, string Content, string MailImageUrl, DateTime? PublishAt, string AuthorName, string Tags, string Categories);