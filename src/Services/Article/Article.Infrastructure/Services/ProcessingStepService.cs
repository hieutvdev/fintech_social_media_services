using Article.Application.DTOs.Request.ProcessingStep;
using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Repositories;
using Article.Application.Services;
using BuildingBlocks.Pagination;
using Marten.Linq.CreatedAt;

namespace Article.Infrastructure.Services;

public class ProcessingStepService(IProcessingStepRepository repository) : IProcessingStepService
{
    public async Task<bool> CreateAsync(CreateProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.UpdateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<IEnumerable<ProcessingStepResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<ProcessingStepResBaseDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<PaginatedResult<ProcessingStepResBaseDto>> GetListAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default)
    {
        return await repository.GetListAsync(filter, cancellationToken);
    }

    public async Task<PaginatedResult<ProcessingStepResBaseDto>> GetByUserAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default)
    {
        return await repository.GetByUserAsync(filter, cancellationToken);
    }
}