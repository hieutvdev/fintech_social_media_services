using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.Block;

namespace User.Application.UseCases.Queries.Block.GetByUserLogin;

public record GetBlockByUserLoginQuery(PaginationRequest Query) : IQuery<PaginatedResult<BlockResBaseDto>>;