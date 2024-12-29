using BuildingBlocks.CQRS.Queries;

namespace User.Application.UseCases.Queries.UserType.Detail;

public record GetDetailUserTypeQuery(int Id) : IQuery<User.Domain.Models.UserType>;