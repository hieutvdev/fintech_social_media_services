using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.UserType;
using User.Application.UseCases.Models.UserType;

namespace User.Application.UseCases.Queries.UserType.GetList;

public record GetListUserTypeQuery(UserTypeSearchListModelQuery Query) : IQuery<PaginatedResult<UserTypeResBaseDto>>; 