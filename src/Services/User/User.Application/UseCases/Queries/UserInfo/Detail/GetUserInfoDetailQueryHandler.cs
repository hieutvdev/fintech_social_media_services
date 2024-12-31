using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Exceptions.ErrorCodeResponse;
using BuildingBlocks.Helpers;
using User.Application.DTOs.Response.UserInfo;
using User.Application.Services;

namespace User.Application.UseCases.Queries.UserInfo.Detail;

public class GetUserInfoDetailQueryHandler
(IUserInfoService service)
: IQueryHandler<GetUserInfoDetailQuery, UserInfoDetailResBaseDto>
{
    public async Task<ResultT<UserInfoDetailResBaseDto>> Handle(GetUserInfoDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetUserInfoByIdAsync(request.Id, cancellationToken);
        var response = result != null
            ? Result.Success(result, "Get user info successfully")
            : Result.Failure<UserInfoDetailResBaseDto>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrNotFound), "Failed to get user info");
        return response;
    }
}