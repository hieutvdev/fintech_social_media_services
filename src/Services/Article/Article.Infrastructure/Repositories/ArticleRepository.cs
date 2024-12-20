using Article.Application.DTOs.Request.Article;
using Article.Application.DTOs.Response.Article;
using Article.Application.Exceptions;
using Article.Application.Mappers.ArticleMapper;
using Article.Application.Repositories;
using Article.Domain.Enums;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using ShredKernel.BaseClasses;
using ShredKernel.Specifications;
using QueryableExtensions = ShredKernel.Specifications.QueryableExtensions;


namespace Article.Infrastructure.Repositories;

public class ArticleRepository(IRepositoryService<string, Article.Domain.Models.Article, ApplicationDbContext> repositoryService,ApplicationDbContext context, IMapper mapper, IAuthorizeExtension authorizeExtension)
: IArticleRepository
{
    public async Task<bool> CreateAsync(CreateArticleRequestDto createArticleRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var articleId = ArticleId.Of(Guid.NewGuid());
            var authorId = AuthorId.Of(Guid.Parse(authorizeExtension.GetUserFromClaimToken().Id));
            
            var res = await repositoryService.ExecuteInTransactionAsync(async () =>
            {
                mapper.Map<Domain.Models.Article>(createArticleRequest);
                var article = Domain.Models.Article.Create(articleId, createArticleRequest.Title, createArticleRequest.Description, createArticleRequest.Content, createArticleRequest.MainImageUrl, authorId);
                article.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
                await repositoryService.AddAsync(article, cancellationToken);
                await repositoryService.SaveChangeAsync(cancellationToken);

                if (createArticleRequest.Categories != null)
                {
                    var categoryIds = createArticleRequest.Categories.Select(r => CategoryId.Of(Guid.Parse(r)));
                    var articleCategoryToAdd = categoryIds.Select(r => ArticleCategory.Create(articleId, r)).ToList();
                    await context.ArticleCategories.AddRangeAsync(articleCategoryToAdd, cancellationToken);
                }
                if (createArticleRequest.Tags != null)
                {
                    var tagIds = createArticleRequest.Tags.Select(r => TagId.Of(Guid.Parse(r)));
                    var articleTagsToAdd = tagIds.Select(r => ArticleTag.Create(articleId, r)).ToList();
                    await context.ArticleTags.AddRangeAsync(articleTagsToAdd, cancellationToken);
                }
            }, cancellationToken);
            return res;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<Domain.Models.Article> DetailAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var parsedId = ArticleId.Of(Guid.Parse(id));

            var articleFound = await repositoryService.FirstOrDefaultAsync(a => a.Id == parsedId, cancellationToken)
                               ?? throw new ArticleNotFoundException("Cannot be to find article");
            
            return articleFound;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<Domain.Models.Article>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await repositoryService.GetAllAsync(cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result =
                await repositoryService.GetSelectorAsync<SelectListItem>(
                    s => new SelectListItem(s.Id.Value.ToString(), s.Title), cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ArticleResponseBaseDto>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var paginatedResult = await repositoryService.GetPageAsync(paginationRequest, null, cancellationToken);
            var articleResponseDto = paginatedResult.DataResponse.Select(r => r.ArticleToBaseDto());
            PaginatedResult<ArticleResponseBaseDto> response = new PaginatedResult<ArticleResponseBaseDto>(
                paginatedResult.PageIndex, paginatedResult.PageSize, paginatedResult.TotalRecord, articleResponseDto);
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ArticleResponseBaseDto>> GetListAsync(
        PaginationRequest paginationRequest, 
        SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
          
            var baseQuery = repositoryService
                .Table().Select(r => new 
                {
                    r.Id,
                    r.Title,
                    r.Slug,
                    r.Description,
                    r.MainImageUrl,
                    r.PublishAt
                })
                .Where(r => r.Title != null).AsQueryable();
            
 
            var detailedQuery = from i in baseQuery
                join at in context.ArticleTags on i.Id equals at.ArticleId into ats
                from at in ats.DefaultIfEmpty()
                join tag in context.Tags.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name
                }).AsNoTracking() on at.TagId equals tag.Id into tags
                from tag in tags.DefaultIfEmpty()
                join ac in context.ArticleCategories on i.Id equals ac.ArticleId into acs
                from ac in acs.DefaultIfEmpty()
                join category in context.Categories.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name
                }).AsNoTracking() on ac.CategoryId equals category.Id into acCategories
                from category in acCategories.DefaultIfEmpty()
                select new
                {
                    i.Id,
                    i.Title,
                    i.Description,
                    i.MainImageUrl,
                    i.PublishAt,
                    i.Slug,
                    TagName = tag.Name,  
                    CategoryName = category.Name,
                };

    
            if (!string.IsNullOrEmpty(searchBaseModel.KeySearch))
            {
                detailedQuery = detailedQuery.Where(r => r.Title.ToLower().Contains(searchBaseModel.KeySearch.ToLower()));
            }


            var groupedQuery = detailedQuery
                .GroupBy(i => i.Id)
                .Select(g => new ArticleResponseBaseDto(
                    g.Key.ToString(),
                    g.First().Title,
                    g.First().Description,
                    g.First().MainImageUrl,
                    g.First().Slug,
                    g.First().PublishAt,
                    string.Join("||", g.Select(x => x.CategoryName).Distinct()),
                    string.Join("||", g.Select(x => x.TagName).Distinct())
                ));

            if (!string.IsNullOrEmpty(searchBaseModel.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<ArticleResponseBaseDto>(searchBaseModel.SortBy);
                groupedQuery = searchBaseModel.IsAscending ? groupedQuery.AsEnumerable().OrderBy(sortBy).AsQueryable() : groupedQuery.AsEnumerable().OrderByDescending(sortBy).AsQueryable();

            }

         
            long totalRecords =  groupedQuery.Count();

       
            var paginatedData =  groupedQuery
                .Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToList();

         
            return new PaginatedResult<ArticleResponseBaseDto>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalRecords,
                paginatedData
            );
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }



    public async Task<bool> DeleteAsync(DeleteArticleRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<string> Ids = requestDto.Ids.Distinct();


            var responseDelete = await repositoryService.ExecuteInTransactionAsync(async () =>
            {
                var articles =
                    await repositoryService.WhereAsync(r => Ids.Contains(r.Id.Value.ToString()), cancellationToken);
                if (articles.Count() != Ids.Count())
                {
                    throw new BadRequestException("Cannot be to find delete data");
                }
                repositoryService.DeleteAsync(articles, cancellationToken);
                
                
            }, cancellationToken);
            return responseDelete;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateArticleRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var articleId = ArticleId.Of(Guid.Parse(requestDto.Id));
            var authorId = AuthorId.Of(Guid.Parse(authorizeExtension.GetUserFromClaimToken().Id));

            return await repositoryService.ExecuteInTransactionAsync(async () =>
            {
               
                var article = await repositoryService.FirstOrDefaultAsync(
                    a => a.Id == articleId, cancellationToken
                ) ?? throw new ArticleNotFoundException("Article not found");

               
                article.Update(requestDto.Title, requestDto.Description, requestDto.Content, requestDto.MainImageUrl, authorId);

                
                if (requestDto.Categories?.Any() == true)
                {
                    await UpdateCategoriesAsync(articleId, requestDto.Categories, cancellationToken);
                }

                
                if (requestDto.Tags?.Any() == true)
                {
                    await UpdateTagsAsync(articleId, requestDto.Tags, cancellationToken);
                }

                repositoryService.Update(article);


            }, cancellationToken);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }



    private async Task UpdateCategoriesAsync(ArticleId articleId, IEnumerable<string> categoryIds, CancellationToken cancellationToken)
    {
        var currentCategories = await context.ArticleCategories
            .Where(r => r.ArticleId == articleId)
            .ToListAsync(cancellationToken);

        var newCategories = categoryIds
            .Distinct()
            .Select(id => CategoryId.Of(Guid.Parse(id)))
            .ToList();

        var categoriesToRemove = currentCategories
            .Where(r => !newCategories.Contains(r.CategoryId))
            .ToList();

        var currentCategoryIds = currentCategories.Select(r => r.CategoryId).ToHashSet();
        var categoriesToAdd = newCategories
            .Where(r => !currentCategoryIds.Contains(r))
            .Select(r => ArticleCategory.Create(articleId, r))
            .ToList();

        if (categoriesToRemove.Any())
        {
            context.ArticleCategories.RemoveRange(categoriesToRemove);
        }

        if (categoriesToAdd.Any())
        {
            await context.ArticleCategories.AddRangeAsync(categoriesToAdd, cancellationToken);
        }
    }

    private async Task UpdateTagsAsync(ArticleId articleId, IEnumerable<string> tagIds, CancellationToken cancellationToken)
    {
        var currentTags = await context.ArticleTags
            .Where(r => r.ArticleId == articleId)
            .ToListAsync(cancellationToken);

        var newTags = tagIds
            .Distinct()
            .Select(id => TagId.Of(Guid.Parse(id)))
            .ToList();

        var tagsToRemove = currentTags
            .Where(r => !newTags.Contains(r.TagId))
            .ToList();

        var currentTagIds = currentTags.Select(r => r.TagId).ToHashSet();
        var tagsToAdd = newTags
            .Where(r => !currentTagIds.Contains(r))
            .Select(r => ArticleTag.Create(articleId, r))
            .ToList();

        if (tagsToRemove.Any())
        {
            context.ArticleTags.RemoveRange(tagsToRemove);
        }

        if (tagsToAdd.Any())
        {
            await context.ArticleTags.AddRangeAsync(tagsToAdd, cancellationToken);
        }
    }
    
    public async Task<ArticleDetailRelateResponseDto> GetRelateDetails(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var articleId = ArticleId.Of(Guid.Parse(id));
            
            var articleFound = await repositoryService.Table()
                .Where(r => r.Id == articleId)
                .Select(r => new
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    MainImageUrl = r.MainImageUrl,
                    Content = r.Content,
                    AuthorId = r.AuthorId.Value,
                    PublishAt = r.PublishAt
                })
                .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new ArticleNotFoundException("Article not found");

         
            var tagIds = await context.ArticleTags
                .Where(t => t.ArticleId == articleId)
                .AsNoTracking()
                .Select(t => t.TagId)
                .ToListAsync(cancellationToken);

            var categoryIds = await context.ArticleCategories
                .Where(c => c.ArticleId == articleId)
                .AsNoTracking()
                .Select(c => c.CategoryId)
                .ToListAsync(cancellationToken);
          
            var categoryNames = await context.Categories
                .Where(r => categoryIds.Contains(r.Id))
                .Select(r => r.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var tagNames = await context.Tags
                .Where(r => tagIds.Contains(r.Id))
                .Select(r => r.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            var articleRelate = await context.Articles
                .Join(context.ArticleCategories,
                    article => article.Id,
                    category => category.ArticleId,
                    (article, category) => new { article, category })
                .Where(joined => categoryIds.Contains(joined.category.CategoryId) && joined.article.Id != articleId)
                .Select(joined => new
                {
                    joined.article.Id,
                    joined.article.Title,
                    joined.article.MainImageUrl,
                    joined.article.PublishAt
                })
                .Distinct()
                .OrderBy(a => a.PublishAt)
                .Take(5)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var articleHots = await repositoryService.Table().Select(r => new
                {
                    Id = r.Id,
                    Title = r.Title,
                    MainImageUrl = r.MainImageUrl,
                    PublishAt = r.PublishAt,
                    Type = r.Type,
                }).Where(r => r.Type == ArticleType.Hot)
                .OrderBy(r => r.PublishAt)
                .AsNoTracking()
                .Take(5)
                .ToListAsync(cancellationToken);
            
            var articleHotsResponse = articleHots.Select(r => new ArticleRelateResponseDto(
                r.Id.Value.ToString(),
                r.Title,
                r.MainImageUrl
            )).ToList();
            
            var articleResponseDto = articleRelate.Select(r => new ArticleRelateResponseDto(
                r.Id.Value.ToString(),
                r.Title,
                r.MainImageUrl
            )).ToList();

         
            var response = new ArticleDetailRelateResponseDto(
                new ArticleGetDetailsResponseDto(
                    articleFound.Id.Value.ToString(),
                    articleFound.Title,
                    articleFound.Description,
                    articleFound.Content,
                    articleFound.MainImageUrl,
                    articleFound.PublishAt,
                    articleFound.AuthorId.ToString(),
                    string.Join("|||", tagNames),
                    string.Join("|||", categoryNames)
                ),
                articleResponseDto,articleHotsResponse
            );

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException($"Error while fetching article details: {e.Message}");
        }
    }
    

    public async Task<PaginatedResult<ArticleByUserResponseDto>> GetArticleByUserAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var authorId = AuthorId.Of(Guid.Parse(userId));
            var articleByUsers = await repositoryService.Table().Select(r => new
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                MainImageUrl = r.MainImageUrl,
                Status = r.Status,
                AuthorId = r.AuthorId,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.PublishAt,
            })
                .Where(r => r.AuthorId == authorId)
                .WhereIf(
                    !string.IsNullOrEmpty(searchBaseModel.KeySearch),
                    r => r.Title.ToLower().Contains(searchBaseModel.KeySearch!.ToLower()) || r.Description.ToLower().Contains(searchBaseModel.KeySearch!.ToLower()))
                .AsNoTracking().ToListAsync(cancellationToken);

            var articleResponse = articleByUsers.Select(r =>
                new ArticleByUserResponseDto(r.Id.Value.ToString(), r.Title, r.Description, r.MainImageUrl,
                    (int)r.Status, r.CreatedAt, r.UpdatedAt));

            if (!string.IsNullOrEmpty(searchBaseModel.SortBy))
            {
                var orderBy = QueryableExtensions.GetPropertyGetter<ArticleByUserResponseDto>(searchBaseModel.SortBy);
                articleResponse = searchBaseModel.IsAscending ? articleResponse.OrderBy(orderBy) : articleResponse.OrderByDescending(orderBy);
            }
            else
            {
                articleResponse = articleResponse.OrderBy(r => r.CreatedAt);
            }

            var byUserResponseDtos = articleResponse.ToList();
            var articleByUserResponseDtos = byUserResponseDtos.ToList();
            
            var totalCount = articleByUserResponseDtos.Count();

            var dataResponse = byUserResponseDtos.Skip(paginationRequest.PageSize * (paginationRequest.PageIndex - 1))
                .Take(paginationRequest.PageSize).ToList();
            
            var paginatedResponse = new PaginatedResult<ArticleByUserResponseDto>(paginationRequest.PageIndex, paginationRequest.PageSize, totalCount, dataResponse);

            return paginatedResponse;

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<ArticleByAdminResponseDto>> GetArticleByAdminAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default)
    {
        try
        {
             var articleByAdmins = await repositoryService.Table().Select(r => new
             {
                 Id = r.Id,
                 Title = r.Title,
                 Description = r.Description,
                 MainImageUrl = r.MainImageUrl,
                 Status = r.Status,
                 AuthorId = r.AuthorId,
                 CreatedAt = r.CreatedAt,
                 UpdatedAt = r.PublishAt,
                 Type = r.Type
                 
             }).WhereIf(
                 !string.IsNullOrEmpty(searchBaseModel.KeySearch),
                 r => r.Title.ToLower().Contains(searchBaseModel.KeySearch!.ToLower()) || r.Description.ToLower().Contains(searchBaseModel.KeySearch!.ToLower())
                 ).AsNoTracking().ToListAsync(cancellationToken);
             
             
             
             var articleResponse = articleByAdmins.Select(r =>
                 new ArticleByAdminResponseDto(r.Id.Value.ToString(), r.Title, r.Description, r.MainImageUrl,r.AuthorId.Value.ToString(),
                     (int)r.Status, r.CreatedAt, r.UpdatedAt, (int)r.Type));

             if (!string.IsNullOrEmpty(searchBaseModel.SortBy))
             {
                 var orderBy = QueryableExtensions.GetPropertyGetter<ArticleByAdminResponseDto>(searchBaseModel.SortBy);
                 articleResponse = searchBaseModel.IsAscending ? articleResponse.OrderBy(orderBy) : articleResponse.OrderByDescending(orderBy);
             }
             else
             {
                 articleResponse = articleResponse.OrderBy(r => r.CreatedAt);
             }

             var articleByAdminResponseDtos = articleResponse.ToList();
             long totalCount = articleByAdminResponseDtos.Count();
             var dataResponse = articleByAdminResponseDtos.Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
                 .Take(paginationRequest.PageSize);

             PaginatedResult<ArticleByAdminResponseDto> response =
                 new PaginatedResult<ArticleByAdminResponseDto>(paginationRequest.PageIndex, paginationRequest.PageSize,
                     totalCount, dataResponse);

             return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> SendRequestArticleByUserAsync(SendRequestArticleRequestDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var articleId = ArticleId.Of(Guid.Parse(payload.ArticleId));
            var status = (ArticleStatus)payload.Status;
            
            if (!ValidateArticleStatus(status))
            {
                return false;
            }
            
            var rowsAffected = await repositoryService.Table()
                .Where(a => a.Id == articleId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Status, status), cancellationToken);

            if (rowsAffected == 0)
            {
                throw new NotFoundException($"Article with ID {payload.ArticleId} not found.");
            }

            return true;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> ConfirmRequestArticleByAdminAsync(ConfirmRequestArticleRequestDto payload,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var articleId = ArticleId.Of(Guid.Parse(payload.ArticleId));
            var status = (ArticleStatus)payload.Status;
            var rowsAffected = await repositoryService.Table()
                .Where(a => a.Id == articleId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Status, status), cancellationToken);

            if (rowsAffected == 0)
            {
                throw new NotFoundException($"Article with ID {payload.ArticleId} not found.");
            }

            return true;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<ArticleRelateResponseDto>> GetArticleRelateByIdAsync(
        string categoryNames, 
        CancellationToken cancellationToken = default)
    {
        try
        {
          
            var names = categoryNames.Split("|||");

           
            var categoryIds = await context.Categories
                .Where(r => names.Contains(r.Name))
                .Select(r => r.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!categoryIds.Any())
            {
                return Enumerable.Empty<ArticleRelateResponseDto>();
            }

        
            var articleRelates = await (
                from i in context.Articles
                join ii in context.ArticleCategories on i.Id equals ii.ArticleId
                where categoryIds.Contains(ii.CategoryId)
                orderby i.PublishAt
                select new ArticleRelateResponseDto(
                    i.Id.ToString(),
                    i.Title,
                    i.MainImageUrl
                )
            ).AsNoTracking().Take(8).ToListAsync(cancellationToken);

            return articleRelates;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }



    private static bool ValidateArticleStatus(ArticleStatus status)
    {
   
        var validStatuses = new[]
        {
            ArticleStatus.Draft,
            ArticleStatus.SubmittedForReview,
            ArticleStatus.RevisionRequested,
            ArticleStatus.RevisedSubmittedForReview
        };
        
        return Enum.IsDefined(typeof(ArticleStatus), status) &&
               !new[]
               {
                   ArticleStatus.NotApproved,
                   ArticleStatus.Approved,
                   ArticleStatus.NotAcceptingRevisions,
                   ArticleStatus.RevisionApproved
               }.Contains(status);
    }
}