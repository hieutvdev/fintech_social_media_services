using Article.Application.DTOs.Response.ArticleRequestPublish;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetAll;

public record GetAllArticleReqPubQuery() : IQuery<IEnumerable<ArticleRequestPublishResBaseDto>>;