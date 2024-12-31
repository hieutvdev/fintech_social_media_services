using BuildingBlocks.CQRS.Queries;
using User.Application.DTOs.Response.UserInfo;

namespace User.Application.UseCases.Queries.UserInfo.Detail;

public record GetUserInfoDetailQuery(string Id) : IQuery<UserInfoDetailResBaseDto>;