using BuildingBlocks.CQRS.Queries;

namespace User.Application.UseCases.Queries.UserType.GetAll;

public record GetAllUserTypeQuery() : IQuery<IEnumerable<Domain.Models.UserType>>;