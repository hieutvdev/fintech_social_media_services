using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.Block;
using User.Application.DTOs.Response.Block;
using User.Application.Repositories;
using User.Application.Services;

namespace User.Infrastructure.Services;

public class BlockService
(IBlockRepository repository)
: IBlockService
{
    public async Task<bool> CreateAsync(CreateBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<PaginatedResult<BlockResBaseDto>> GetBlocksByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        return await repository.GetBlocksByUserLoginAsync(paginationRequest, cancellationToken);
    }

    public async Task<bool> CheckBlockAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await repository.CheckBlockAsync(userId, cancellationToken);
    }
}