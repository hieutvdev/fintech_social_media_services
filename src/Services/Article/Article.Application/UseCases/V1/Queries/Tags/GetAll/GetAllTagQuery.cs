using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetAll;

public record GetAllTagQuery() : IQuery<IEnumerable<Tag>>;