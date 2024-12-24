using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetByUser;

public class GetByUserArticleReqPubQueryHandler
(IArticleRequestPublishService service) : IQueryHandler<GetByUserArticleReqPubQuery, PaginatedResult<ArticleRequestPublishResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleRequestPublishResBaseDto>>> Handle(GetByUserArticleReqPubQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetByUserAsync(request.Query, cancellationToken);
        var response = Result.Create(result);
        return response;
        
    }
}