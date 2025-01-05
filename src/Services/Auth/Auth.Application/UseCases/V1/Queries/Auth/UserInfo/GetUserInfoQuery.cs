using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Queries.Auth.UserInfo;

public record GetUserInfoQuery(string UserId) : IQuery<UserDto>;