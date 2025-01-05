using User.Application.DTOs.Request.UserInfo;
using User.Application.DTOs.Response.UserInfo;
using User.Application.Repositories;
using User.Application.Services;
using User.Domain.Models;

namespace User.Infrastructure.Services;

public class UserInfoService
(IUserInfoRepository repository)
: IUserInfoService
{
    public async Task<bool> CreateUserInfoAsync(CreateUserInfoReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateUserInfoAsync(payload, cancellationToken);
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.UpdateUserInfoAsync(payload, cancellationToken);
    }

    public async Task<UserInfoDetailResBaseDto> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await repository.GetUserInfoByIdAsync(id, cancellationToken);
    }

    public async Task<bool> DeleteUserInfoAsync(DeleteUserInfoReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteUserInfoAsync(payload, cancellationToken);
    }
}