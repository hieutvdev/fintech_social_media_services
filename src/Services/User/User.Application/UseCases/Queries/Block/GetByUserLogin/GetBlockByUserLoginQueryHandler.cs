using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.Block;
using User.Application.Services;

namespace User.Application.UseCases.Queries.Block.GetByUserLogin;

public class GetBlockByUserLoginQueryHandler
(IBlockService service)
: IQueryHandler<GetBlockByUserLoginQuery, PaginatedResult<BlockResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<BlockResBaseDto>>> Handle(GetBlockByUserLoginQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetBlocksByUserLoginAsync(request.Query, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}