using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetSelect;

public record GetSelectArticleQuery() : IQuery<IEnumerable<SelectListItem>>;