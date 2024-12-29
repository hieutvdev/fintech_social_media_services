using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using User.Application.Services;

namespace User.Application.UseCases.Queries.UserType.GetAll;

public class GetAllUserTypeQueryHandler
(IUserTypeService service)
: IQueryHandler<GetAllUserTypeQuery, IEnumerable<Domain.Models.UserType>>
{
    public async Task<ResultT<IEnumerable<Domain.Models.UserType>>> Handle(GetAllUserTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}