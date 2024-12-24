using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetAll;

public class GetAllArticleReqPubQueryHandler
(IArticleRequestPublishService service) : IQueryHandler<GetAllArticleReqPubQuery, IEnumerable<ArticleRequestPublishResBaseDto>>
{
    public async Task<ResultT<IEnumerable<ArticleRequestPublishResBaseDto>>> Handle(GetAllArticleReqPubQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}