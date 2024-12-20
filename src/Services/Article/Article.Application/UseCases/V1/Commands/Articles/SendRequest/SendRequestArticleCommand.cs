using Article.Application.DTOs.Request.Article;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Articles.SendRequest;

public record SendRequestArticleCommand(SendRequestArticleRequestDto Request) : ICommand<bool>;