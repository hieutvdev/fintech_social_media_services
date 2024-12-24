using Article.Application.DTOs.Request.ArticleRequestPublish;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Update;

public record UpdateArticleRequestPublishCommand(UpdateArticleRequestPublishDto Payload) : ICommand<bool>;