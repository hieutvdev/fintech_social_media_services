using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Queries.Auth.GetUserShare;

public record GetUserShareQuery(UserShareRequestDto Query) : IQuery<IEnumerable<UserShareResponseDto>>;