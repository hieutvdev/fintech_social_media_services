using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.Detail;

public record GetDetailArticleQuery(string Id) : IQuery<Article.Domain.Models.Article>;