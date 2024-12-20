using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetSelect;

public record GetSelectTagQuery() : IQuery<IEnumerable<SelectListItem>>;