using User.Application.DTOs.Request.Follow;

namespace User.Application.Repositories;

public interface IFollowRepository
{
    Task<bool> CreateAsync(CreateFollowReqDto payload, CancellationToken cancellationToken = default!);
    Task<bool> DeleteAsync(DeleteFollowReqDto payload, CancellationToken cancellationToken = default!);
    Task<bool> CheckFollowAsync(string userId, CancellationToken cancellationToken = default!);
    
}