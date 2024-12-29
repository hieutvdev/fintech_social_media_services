using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;
using User.Application.DTOs.Request.UserType;
using User.Application.DTOs.Response.UserType;
using User.Application.UseCases.Models.UserType;
using User.Domain.Models;

namespace User.Application.Services;

public interface IUserTypeService
{
    
    Task<IEnumerable<UserType>> GetAllAsync(CancellationToken cancellationToken = default!);

    Task<UserType> DetailsAsync(int id, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<UserTypeResBaseDto>> GetListAsync(UserTypeSearchListModelQuery query, CancellationToken cancellationToken = default!);
    
    Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default!);
    
    Task<bool> CreateAsync(CreateUserTypeReqDto payload, CancellationToken cancellationToken = default!);

    Task<bool> UpdateAsync(UpdateUserTypeReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteAsync(DeleteUserTypeReqDto payload, CancellationToken cancellationToken = default!);
}