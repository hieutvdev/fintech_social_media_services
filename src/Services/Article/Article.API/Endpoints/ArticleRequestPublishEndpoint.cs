using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Create;
using Article.Application.UseCases.V1.Queries.ArticleRequestPublish.Detail;
using Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetList;

namespace Article.API.Endpoints;

public class ArticleRequestPublishEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/article-request-publish";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("article-request-publish")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group.MapPost("/", async (CreateArticleRequestPublishDto model, ISender sender) =>
            {
                var result = await sender.Send(new CreateArticleRequestPublishCommand(model));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("CreateArticleRequestPublish")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create ArticleRequestPublish")
            .WithDescription("Create ArticleRequestPublish")
            .RequireAuthorization();
        
        
        group.MapPost("/get-detail/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetDetailArticleReqPubQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("DetailArticleRequestPublish")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Detail ArticleRequestPublish")
            .WithDescription("Detail ArticleRequestPublish")
            .RequireAuthorization();
        
        
        group.MapGet("/get-list", async ([AsParameters] ArticleReqSearchListDto query,ISender sender) =>
            {
               
                var result = await sender.Send(new GetListArticleReqPubQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetListArticleRequestPublish")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetList ArticleRequestPublish")
            .WithDescription("GetList ArticleRequestPublish")
            .RequireAuthorization();
    }
}