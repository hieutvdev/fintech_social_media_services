using Article.Application.DTOs.Request.ProcessingStep;
using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Exceptions;
using Article.Application.Repositories;
using Article.Domain.Enums;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using Marten;
using ShredKernel.Specifications;
using QueryableExtensions = ShredKernel.Specifications.QueryableExtensions;

namespace Article.Infrastructure.Repositories;

public class ProcessingStepRepository(IRepositoryBaseService<ApplicationDbContext> repository, IAuthorizeExtension authorizeExtension) : IProcessingStepRepository
{
    public async Task<bool> CreateAsync(CreateProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var processingStepId = ProcessingStepId.Of(Guid.NewGuid());
            var articleReqPubId = ArticleRequestPublishId.Of(Guid.Parse(payload.ArticleRequestPublishId));
            ProcessingStepStatus status = (ProcessingStepStatus)payload.Status;
            var processingStep = ProcessingStep.Create(processingStepId, articleReqPubId, payload.Description,
                payload.HandlerId,status);
            processingStep.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
            
            await repository.AddAsync<ProcessingStep>(processingStep, cancellationToken);
            var isSuccess = await repository.SaveChangesAsync(cancellationToken) > 0;
            
            return isSuccess;
            
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload), "Payload cannot be null");
        }

        if (string.IsNullOrEmpty(payload.Id))
        {
            throw new ArgumentException("Processing Step Id cannot be null or empty", nameof(payload.Id));
        }
        
        var handlerId = authorizeExtension.GetUserFromClaimToken().Id;
        try
        {
           var processingStepId = ProcessingStepId.Of(Guid.Parse(payload.Id));

           var isUpdated = await repository.ExecuteUpdateAsync<ProcessingStep>(
               condition: r => r.Id == processingStepId,
               updateExpression: updates => updates
                   .SetProperty(r => r.Description, payload.Description)
                   .SetProperty(r => r.Status, payload.Status)
                   .SetProperty(r => r.HandlerId, handlerId),
               cancellationToken: cancellationToken);

           return isUpdated;

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteProcessingRequestDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var ids = payload.Ids.Distinct();
            var processingStepIds = ids.Select(r => ProcessingStepId.Of(Guid.Parse(r))).ToList();
            bool isDeleted = await repository.ExecuteDeleteAsync<ProcessingStep>(p => processingStepIds.Contains(p.Id), cancellationToken);
            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<ProcessingStepResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var processingSteps = await repository.Table<ProcessingStep>().Select(r => new
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status,
                ArticleReqId = r.ArticleRequestPublishId,
                HandlerId = r.HandlerId
            }).ToListAsync(cancellationToken);
            
            return processingSteps.Select(r => new ProcessingStepResBaseDto(r.Id.Value.ToString(), r.Description, r.Status, r.ArticleReqId.Value.ToString(), r.HandlerId.ToString()));
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<ProcessingStepResBaseDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var processingStepId = ProcessingStepId.Of(Guid.Parse(id));

            var processingStepFound =
                await repository.FirstOrDefaultAsync<ProcessingStep>(r => r.Id == processingStepId, cancellationToken);
            if (processingStepFound is null)
            {
                throw new ProcessingStepNotFoundException("Processing Step Not Found");
            }
            
            return new ProcessingStepResBaseDto(processingStepFound.Id.Value.ToString(), processingStepFound.Description, processingStepFound.Status, processingStepFound.ArticleRequestPublishId.Value.ToString(), processingStepFound.HandlerId.ToString());
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ProcessingStepResBaseDto>> GetListAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
            var baseQuery = repository.Table<ProcessingStep>().Select(r => new
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status,
                ArticleReqId = r.ArticleRequestPublishId,
                HandlerId = r.HandlerId
            }).WhereIf(!string.IsNullOrEmpty(filter.KeySearch), r => r.Description.ToLower().Contains(filter.KeySearch!));

            if (!string.IsNullOrEmpty(filter.AritcleReqPubId))
            {
                var articleReqPubId = ArticleRequestPublishId.Of(Guid.Parse(filter.AritcleReqPubId));
                baseQuery = from i in baseQuery
                    where i.ArticleReqId == articleReqPubId
                    select i;
            }

            if (!string.IsNullOrEmpty(filter.HandlerId))
            {
                baseQuery = from i in baseQuery
                    where i.HandlerId == filter.HandlerId
                    select i;
            }
            
            var resultDto = baseQuery.Select(r=> new ProcessingStepResBaseDto(r.Id.Value.ToString(), r.Description, r.Status, r.ArticleReqId.Value.ToString(), r.HandlerId.ToString())).AsEnumerable();

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<ProcessingStepResBaseDto>(filter.SortBy);
                resultDto = filter.IsAscending == true ? resultDto.OrderBy(sortBy) : resultDto.OrderByDescending(sortBy);
            }
            else
            {
                resultDto = resultDto.OrderBy(r => r.Description);
            }

            var processingStepDto = resultDto.ToList();
            var totalRecord = processingStepDto.Count();
            
            var paginationReq = new PaginationRequest(filter.PageIndex ?? 1, filter.PageSize ?? 10);
            var paginatedData = processingStepDto.Skip((int)((paginationReq.PageIndex - 1) * paginationReq.PageSize)!).Take((int)paginationReq.PageSize!);
            
            return new PaginatedResult<ProcessingStepResBaseDto>(filter.PageIndex ?? 1, filter.PageSize ?? 20, totalRecord, paginatedData);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ProcessingStepResBaseDto>> GetByUserAsync(ProcessingStepSearchListDto filter, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
            var handlerId = authorizeExtension.GetUserFromClaimToken().Id;
            var baseQuery = repository.Table<ProcessingStep>().Select(r => new
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status,
                ArticleReqId = r.ArticleRequestPublishId,
                HandlerId = r.HandlerId
            }).WhereIf(!string.IsNullOrEmpty(filter.KeySearch), r => r.Description.ToLower().Contains(filter.KeySearch!))
            .Where(r => r.HandlerId == handlerId);

            if (!string.IsNullOrEmpty(filter.AritcleReqPubId))
            {
                var articleReqPubId = ArticleRequestPublishId.Of(Guid.Parse(filter.AritcleReqPubId));
                baseQuery = from i in baseQuery
                    where i.ArticleReqId == articleReqPubId
                    select i;
            }

            var resultDto = baseQuery.Select(r=> new ProcessingStepResBaseDto(r.Id.Value.ToString(), r.Description, r.Status, r.ArticleReqId.Value.ToString(), r.HandlerId.ToString())).AsEnumerable();

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<ProcessingStepResBaseDto>(filter.SortBy);
                resultDto = filter.IsAscending == true ? resultDto.OrderBy(sortBy) : resultDto.OrderByDescending(sortBy);
            }
            else
            {
                resultDto = resultDto.OrderBy(r => r.Description);
            }

            var processingStepDto = resultDto.ToList();
            var totalRecord = processingStepDto.Count();
            
            var paginationReq = new PaginationRequest(filter.PageIndex ?? 1, filter.PageSize ?? 10);
            var paginatedData = processingStepDto.Skip((int)((paginationReq.PageIndex - 1) * paginationReq.PageSize)!).Take((int)paginationReq.PageSize!);
            
            return new PaginatedResult<ProcessingStepResBaseDto>(filter.PageIndex ?? 1, filter.PageSize ?? 20, totalRecord, paginatedData);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}