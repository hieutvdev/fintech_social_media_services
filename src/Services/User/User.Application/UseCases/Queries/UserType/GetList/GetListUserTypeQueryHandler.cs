using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.UserType;
using User.Application.Services;

namespace User.Application.UseCases.Queries.UserType.GetList;

public class GetListUserTypeQueryHandler
(IUserTypeService service)
: IQueryHandler<GetListUserTypeQuery, PaginatedResult<UserTypeResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<UserTypeResBaseDto>>> Handle(GetListUserTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(request.Query, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}