using Article.Application.DTOs.Request.ProcessingStep;
using Article.Application.DTOs.Response.ProcessingStep;

namespace Article.Application.Services;

public interface IProcessingStepService
{
    Task<bool> CreateAsync(CreateProcessingRequestDto payload, CancellationToken cancellationToken = default!);
    Task<bool> UpdateAsync(UpdateProcessingRequestDto payload, CancellationToken cancellationToken = default!);
    Task<bool> DeleteAsync(DeleteProcessingRequestDto payload, CancellationToken cancellationToken = default!);
    Task<IEnumerable<ProcessingStepResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default!);
    Task<ProcessingStepResBaseDto> GetByIdAsync(string id, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<ProcessingStepResBaseDto>> GetListAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<ProcessingStepResBaseDto>> GetByUserAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default!);
}