using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetPage;

public record GetPageTagQuery(PaginationRequest PaginationRequest) : IQuery<PaginatedResult<Tag>>;