namespace Article.Application.DTOs.Request.ArticleRequestPublish;

public record UpdateArticleRequestPublishDto(string Id, string Name, string Description, string ArticleId, int Status);