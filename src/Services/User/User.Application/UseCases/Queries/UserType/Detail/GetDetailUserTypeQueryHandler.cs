using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using User.Application.Services;

namespace User.Application.UseCases.Queries.UserType.Detail;

public class GetDetailUserTypeQueryHandler
(IUserTypeService service)
: IQueryHandler<GetDetailUserTypeQuery, User.Domain.Models.UserType>
{
    public async Task<ResultT<Domain.Models.UserType>> Handle(GetDetailUserTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await service.DetailsAsync(request.Id, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}