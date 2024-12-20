using Article.Application.DTOs.Request.Article;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Articles.Delete;

public record DeleteArticleCommand(DeleteArticleRequestDto DeleteArticleRequestDto) : ICommand<bool>;