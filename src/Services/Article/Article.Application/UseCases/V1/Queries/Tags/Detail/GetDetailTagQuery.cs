using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.Detail;

public record GetDetailTagQuery(string Id) : IQuery<Tag>;