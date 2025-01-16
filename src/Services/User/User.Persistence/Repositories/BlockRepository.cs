using User.Application.DTOs.Request.Block;
using User.Application.DTOs.Response.Block;

namespace User.Persistence.Repositories;

public class BlockRepository
(IAuthorizeExtension authorizeExtension, IRepositoryBaseService<ApplicationDbContext> repository, IDistributedCacheService cacheService)
: IBockRepository
{
    public Task<bool> CreateAsync(CreateBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(DeleteBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<BlockResBaseDto>> GetBlocksByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}