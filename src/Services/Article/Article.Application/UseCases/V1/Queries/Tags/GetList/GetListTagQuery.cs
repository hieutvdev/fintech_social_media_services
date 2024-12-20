using Article.Application.DTOs.Response.Tag;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetList;

public record GetListTagQuery(PaginationRequest PaginationRequest, SearchBaseModel SearchBaseModel) : IQuery<PaginatedResult<TagResponseBaseDto>>;