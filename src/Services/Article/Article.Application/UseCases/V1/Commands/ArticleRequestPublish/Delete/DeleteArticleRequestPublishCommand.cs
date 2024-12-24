
using Article.Application.DTOs.Request.ArticleRequestPublish;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Delete;

public record DeleteArticleRequestPublishCommand(DeleteArticleRequestPublishDto Payload) : ICommand<bool>;