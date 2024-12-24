using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetList;

public class GetListArticleReqPubQueryHandler(IArticleRequestPublishService service)
: IQueryHandler<GetListArticleReqPubQuery, PaginatedResult<ArticleRequestPublishResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleRequestPublishResBaseDto>>> Handle(GetListArticleReqPubQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(request.Search, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}