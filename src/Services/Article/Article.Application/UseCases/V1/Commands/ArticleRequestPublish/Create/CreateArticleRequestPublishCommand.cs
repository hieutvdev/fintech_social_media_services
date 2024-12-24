using Article.Application.DTOs.Request.ArticleRequestPublish;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Create;

public record CreateArticleRequestPublishCommand(CreateArticleRequestPublishDto Payload) : ICommand<bool>;