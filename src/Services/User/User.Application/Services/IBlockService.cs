using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.Block;
using User.Application.DTOs.Response.Block;

namespace User.Application.Services;

public interface IBlockService
{
    Task<bool> CreateAsync(CreateBlockReqDto payload, CancellationToken cancellationToken = default!);
    Task<bool> DeleteAsync(DeleteBlockReqDto payload, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<BlockResBaseDto>> GetBlocksByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);  
    Task<bool> CheckBlockAsync(string userId, CancellationToken cancellationToken = default!);

}