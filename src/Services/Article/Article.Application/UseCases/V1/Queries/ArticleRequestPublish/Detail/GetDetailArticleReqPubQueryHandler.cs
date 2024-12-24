using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.Detail;

public class GetDetailArticleReqPubQueryHandler
(IArticleRequestPublishService service) : IQueryHandler<GetDetailArticleReqPubQuery, ArticleReqPublishDetailResDto>
{
    public async Task<ResultT<ArticleReqPublishDetailResDto>> Handle(GetDetailArticleReqPubQuery request, CancellationToken cancellationToken)
    {
        var result = await service.DetailAsync(request.Id, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}