using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using ShredKernel.BaseClasses;
using User.Application.Services;

namespace User.Application.UseCases.Queries.UserType.GetSelect;

public class GetSelectUserTypeQueryHandler
(IUserTypeService service)
: IQueryHandler<GetSelectUserTypeQuery, IEnumerable<SelectListItem>>
{
    public async Task<ResultT<IEnumerable<SelectListItem>>> Handle(GetSelectUserTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetSelectAsync(cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
    
}