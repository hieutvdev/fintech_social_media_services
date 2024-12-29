using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;
using User.Application.DTOs.Request.UserType;
using User.Application.DTOs.Response.UserType;
using User.Application.Repositories;
using User.Application.Services;
using User.Application.UseCases.Models.UserType;
using User.Domain.Models;

namespace User.Infrastructure.Services;

public class UserTypeService(IUserTypeRepository repository) : IUserTypeService
{
    public async Task<IEnumerable<UserType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<UserType> DetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await repository.DetailsAsync(id, cancellationToken);
    }

    public async Task<PaginatedResult<UserTypeResBaseDto>> GetListAsync(UserTypeSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetSelectAsync(cancellationToken);
    }

    public async Task<bool> CreateAsync(CreateUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.UpdateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }
}