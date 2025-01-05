using User.Application.DTOs.Response.AuthService;

namespace User.Application.Services;

public interface IAuthServerService
{
    Task<IEnumerable<UserShareResDto>> GetUserShareAsync(IEnumerable<string> Ids, CancellationToken cancellationToken);
}