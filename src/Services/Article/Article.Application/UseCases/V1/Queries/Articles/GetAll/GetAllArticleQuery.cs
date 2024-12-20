using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetAll;

public record GetAllArticleQuery() : IQuery<IEnumerable<Domain.Models.Article>>;