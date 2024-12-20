using Article.Application.DTOs.Request.Article;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Articles.Create;

public record CreateArticleCommand(CreateArticleRequestDto CreateArticleRequestDto) : ICommand<bool>;