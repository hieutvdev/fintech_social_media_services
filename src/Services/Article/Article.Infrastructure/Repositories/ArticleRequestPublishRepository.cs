
using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Exceptions;
using Article.Application.Repositories;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using ShredKernel.Specifications;

namespace Article.Infrastructure.Repositories;

public class ArticleRequestPublishRepository(IRepositoryBaseService<ApplicationDbContext> repositoryService, IAuthorizeExtension authorizeExtension) : IArticleRequestPublishRepository
{
    public async Task<bool> CreateAsync(CreateArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            ArticleRequestPublishId articleRequestPublishId = ArticleRequestPublishId.Of(Guid.NewGuid());
            ArticleId articleId = ArticleId.Of(Guid.Parse(payload.ArticleId));
            ArticleRequestPublish articleRequestPublish =  ArticleRequestPublish.Create(articleRequestPublishId, articleId, payload.Name, payload.Description);
            articleRequestPublish.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
            await repositoryService.AddAsync<ArticleRequestPublish>(articleRequestPublish, cancellationToken);
            var isSaved = await repositoryService.SaveChangesAsync(cancellationToken) > 0;
            return isSaved;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<ArticleRequestPublishResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var articleRequestPublishes = await repositoryService.Table<ArticleRequestPublish>().ToListAsync(cancellationToken);
            return articleRequestPublishes.Select(a => new ArticleRequestPublishResBaseDto
            (
               a.Id.Value.ToString(), 
               a.Name,
               a.Description,
               a.ArticleId.Value.ToString(),
               a.Status,
               a.CreatedBy!
            ));
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<ArticleReqPublishDetailResDto> DetailAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var articleReqPublishId = ArticleRequestPublishId.Of(Guid.Parse(id));
            var articleFound =
                await repositoryService.FirstOrDefaultAsync<ArticleRequestPublish>(a => a.Id == articleReqPublishId,
                    cancellationToken);
            if (articleFound is null)
            {
                throw new ArticleReqPubNotFoundException("Article Request Publish not found");
            }

            var processingSteps =
                await repositoryService.WhereAsync<ProcessingStep>(r => r.ArticleRequestPublishId == articleFound.Id);

            return new ArticleReqPublishDetailResDto(
                ArticleRequestPublish: 
                new ArticleRequestPublishResBaseDto(articleFound.Id.Value.ToString(), articleFound.Name, articleFound.Description, articleFound.ArticleId.Value.ToString(),articleFound.Status ,articleFound.CreatedBy! ),
                ProcessingSteps: processingSteps.Any()
                    ? processingSteps.Select(r =>
                        new ProcessingStepResByReqDto(r.Id.Value.ToString(), r.Description, "", r.Status, r.HandlerId))
                    : Enumerable.Empty<ProcessingStepResByReqDto>());
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetListAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
            var baseRequest = repositoryService.Table<ArticleRequestPublish>().Select(
                r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    ArticleId = r.ArticleId,
                    Status = r.Status,
                    CreatedBy = r.CreatedBy,
                });

            if (!string.IsNullOrEmpty(query.KeySearch))
            {
                baseRequest = from i in baseRequest
                    where i.Name.Contains(query.KeySearch) || i.Description.Contains(query.KeySearch)
                    select i;
            }

            if (!string.IsNullOrEmpty(query.ArticleId))
            {
                var articleId = ArticleId.Of(Guid.Parse(query.ArticleId));
                baseRequest = from i in baseRequest
                    where i.ArticleId == articleId
                    select i;
            }

            if (!string.IsNullOrEmpty(query.HandlerId))
            {
                baseRequest = from i in baseRequest
                    where i.CreatedBy == query.HandlerId
                    select i;
            }
            
            var resultDto = baseRequest.Select(r => new ArticleRequestPublishResBaseDto(r.Id.Value.ToString(), r.Name, r.Description, r.ArticleId.Value.ToString(), r.Status, r.CreatedBy!)).AsEnumerable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<ArticleRequestPublishResBaseDto>(query.SortBy);
                resultDto = query.IsAscending == true ? resultDto.OrderBy(sortBy) : resultDto.OrderByDescending(sortBy);
            }
            else
            {
                resultDto = resultDto.OrderBy(r => r.Name);
            }

            var articleRequestPublishResBaseDtos = resultDto.ToList();
            var totalRecord = articleRequestPublishResBaseDtos.Count();

            var paginationReq = new PaginationRequest(query.PageIndex ?? 1, query.PageSize ?? 10);
            var paginatedData = articleRequestPublishResBaseDtos.Skip((int)((paginationReq.PageIndex - 1) * paginationReq.PageSize)!).Take((int)paginationReq.PageSize!);
            
            return new PaginatedResult<ArticleRequestPublishResBaseDto>(query.PageIndex ?? 1, query.PageSize ?? 20, totalRecord, paginatedData);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetByUserAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
            var handlerId = authorizeExtension.GetUserFromClaimToken().Id;
            var baseRequest = repositoryService.Table<ArticleRequestPublish>().Select(
                r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    ArticleId = r.ArticleId,
                    Status = r.Status,
                    CreatedBy = r.CreatedBy,
                })
                    .Where(r => r.CreatedBy == handlerId)
                    .WhereIf(!string.IsNullOrEmpty(query.KeySearch), r => r.Name.Contains(query.KeySearch!) || r.Description.Contains(query.KeySearch!));

            if (!string.IsNullOrEmpty(query.ArticleId))
            {
                var articleId = ArticleId.Of(Guid.Parse(query.ArticleId));
                baseRequest = from i in baseRequest
                    where i.ArticleId == articleId
                    select i;
            }
            
            
            var resultDto = baseRequest.Select(r => new ArticleRequestPublishResBaseDto(r.Id.Value.ToString(), r.Name, r.Description, r.ArticleId.Value.ToString(), r.Status, r.CreatedBy!)).AsEnumerable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<ArticleRequestPublishResBaseDto>(query.SortBy);
                resultDto = query.IsAscending == true ? resultDto.OrderBy(sortBy) : resultDto.OrderByDescending(sortBy);
            }
            else
            {
                resultDto = resultDto.OrderBy(r => r.Name);
            }

            var articleRequestPublishResBaseDtos = resultDto.ToList();
            var totalRecord = articleRequestPublishResBaseDtos.Count();

            var paginationReq = new PaginationRequest(query.PageIndex ?? 1, query.PageSize ?? 10);
            var paginatedData = articleRequestPublishResBaseDtos.Skip((int)((paginationReq.PageIndex - 1) * paginationReq.PageSize)!).Take((int)paginationReq.PageSize!);
            
            return new PaginatedResult<ArticleRequestPublishResBaseDto>(query.PageIndex ?? 1, query.PageSize ?? 20, totalRecord, paginatedData);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }


    public async Task<bool> DeleteAsync(DeleteArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var ids = payload.Ids.Distinct();
            var articleRequestPublishIds = ids.Select(id => ArticleRequestPublishId.Of(Guid.Parse(id)));
            bool isDeleted = await repositoryService.ExecuteDeleteAsync<ArticleRequestPublish>(r => articleRequestPublishIds.Contains(r.Id), cancellationToken);
            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload), "Payload cannot be null.");
        }

        if (string.IsNullOrEmpty(payload.Id))
        {
            throw new ArgumentException("Article Request Publish Id cannot be null or empty.", nameof(payload.Id));
        }

        try
        {
            
            var articleRequestPublishId = ArticleRequestPublishId.Of(Guid.Parse(payload.Id));
            var articleId = ArticleId.Of(Guid.Parse(payload.ArticleId));
        
            var isUpdated = await repositoryService.ExecuteUpdateAsync<ArticleRequestPublish>(
                condition: r => r.Id == articleRequestPublishId,
                updateExpression: updates => updates
                    .SetProperty(r => r.Name, payload.Name)
                    .SetProperty(r => r.Description, payload.Description)
                    .SetProperty(r => r.Status, payload.Status)
                    .SetProperty(r => r.ArticleId, articleId),
                cancellationToken: cancellationToken);

            
            return isUpdated;
        }
        catch (Exception ex)
        {
            
            throw new BadRequestException($"An error occurred while updating the article request publish: {ex.Message}", ex.Message);
        }
    }
}