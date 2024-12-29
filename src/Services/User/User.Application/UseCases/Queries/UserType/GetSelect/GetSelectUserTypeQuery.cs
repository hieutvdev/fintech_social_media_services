using BuildingBlocks.CQRS.Queries;
using ShredKernel.BaseClasses;

namespace User.Application.UseCases.Queries.UserType.GetSelect;

public record GetSelectUserTypeQuery() : IQuery<IEnumerable<SelectListItem>>;