using Article.Application.DTOs.Request.Article;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Articles.ConfirmRequest;

public record ConfirmRequestArticleCommand(ConfirmRequestArticleRequestDto Request) : ICommand<bool>;