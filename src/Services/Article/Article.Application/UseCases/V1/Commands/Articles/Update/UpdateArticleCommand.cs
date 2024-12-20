using Article.Application.DTOs.Request.Article;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Articles.Update;

public record UpdateArticleCommand(UpdateArticleRequestDto Request) : ICommand<bool>;