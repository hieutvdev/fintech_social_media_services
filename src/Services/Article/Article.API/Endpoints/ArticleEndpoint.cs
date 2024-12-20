using Article.Application.DTOs.Request.Article;
using Article.Application.UseCases.V1.Commands.Articles.ConfirmRequest;
using Article.Application.UseCases.V1.Commands.Articles.Create;
using Article.Application.UseCases.V1.Commands.Articles.Delete;
using Article.Application.UseCases.V1.Commands.Articles.SendRequest;
using Article.Application.UseCases.V1.Commands.Articles.Update;
using Article.Application.UseCases.V1.Queries.Articles.ByAdmin;
using Article.Application.UseCases.V1.Queries.Articles.ByAuthor;
using Article.Application.UseCases.V1.Queries.Articles.Detail;
using Article.Application.UseCases.V1.Queries.Articles.DetailRelated;
using Article.Application.UseCases.V1.Queries.Articles.GetAll;
using Article.Application.UseCases.V1.Queries.Articles.GetList;
using Article.Application.UseCases.V1.Queries.Articles.GetPage;
using Article.Application.UseCases.V1.Queries.Articles.GetSelect;
using Article.Application.UseCases.V1.Queries.Categories.Detail;
using BuildingBlocks.Pagination;
using BuildingBlocks.Utilities.ParamExtensions;
using Microsoft.AspNetCore.Mvc;
using ShredKernel.BaseClasses;

namespace Article.API.Endpoints;

public class ArticleEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/articles";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("articles")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        
        group.MapPost("/", async (CreateArticleRequestDto model, ISender sender) =>
            {
                var result = await sender.Send(new CreateArticleCommand(model));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("CreateArticle")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Article")
        .WithDescription("Create Article")
        .RequireAuthorization();
        
        
        group.MapGet("/", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllArticleQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetAllArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetAll Article")
            .WithDescription("GetAll Article")
            .RequireAuthorization();
        
        group.MapGet("/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetDetailArticleQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
        .WithName("GetDetailArticle")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("GetDetail Article")
        .WithDescription("GetDetail Article")
        .RequireAuthorization();
        
        
        group.MapGet("/get-page", async ([AsParameters] PaginationRequest query, ISender sender) =>
            {
                var result = await sender.Send(new GetPageArticleQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetPageArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetPage Article")
            .WithDescription("GetPage Article")
            .RequireAuthorization();
        
        group.MapGet("/get-select", async (ISender sender) =>
            {
                var result = await sender.Send(new GetSelectArticleQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetSelectArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetSelect Article")
            .WithDescription("GetSelect Article")
            .RequireAuthorization();
        
        group.MapGet("/get-list", async ([AsParameters] SearchListModel query,ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(query);
                var result = await sender.Send(new GetListArticleQuery(paginationRequest, searchBaseModel));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetListArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetList Article")
            .WithDescription("GetList Article")
            .RequireAuthorization();
        
        group.MapGet("/get-detail/{id}", async (string id,ISender sender) =>
            {
               
                var result = await sender.Send(new DetailRelatedArticleQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetDetailRelateArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Detail Relate Article")
            .WithDescription("Get Detail Relate Article")
            .RequireAuthorization();
        
        group.MapGet("/get-by-user", async ([AsParameters] SearchListModel searchListModel ,ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(searchListModel);
                var result = await sender.Send(new ByAuthorArticleQuery(paginationRequest, searchBaseModel));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetByAuthorArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get By Author Article")
            .WithDescription("Get By Author Article")
            .RequireAuthorization();
        
        group.MapGet("/get-by-admin", async ([AsParameters] SearchListModel searchListModel ,ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(searchListModel);
                var result = await sender.Send(new ByAdminArticleQuery(paginationRequest, searchBaseModel));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetByAdminArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get By Admin Article")
            .WithDescription("Get By Admin Article")
            .RequireAuthorization();
        
        group.MapGet("/get-article-relate-name", async ([AsParameters] SearchListModel searchListModel ,ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(searchListModel);
                var result = await sender.Send(new ByAdminArticleQuery(paginationRequest, searchBaseModel));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetRelateArticleByCategoryName")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Relate Article By Category Name")
            .WithDescription("Get Relate Article By Category Name")
            .RequireAuthorization();
        
        
        group.MapPut("/", async ([FromBody] UpdateArticleRequestDto payload,ISender sender) =>
            {
                var result = await sender.Send(new UpdateArticleCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("UpdateArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Article")
            .WithDescription("Update Article")
            .RequireAuthorization();
        
        group.MapPatch("/send-request", async ([FromBody] SendRequestArticleRequestDto payload,ISender sender) =>
            {
                var result = await sender.Send(new SendRequestArticleCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("SendRequestArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Send Request Article")
            .WithDescription("Send Request Article")
            .RequireAuthorization();
        
        group.MapPatch("/confirm-request", async ([FromBody] ConfirmRequestArticleRequestDto payload,ISender sender) =>
            {
                var result = await sender.Send(new ConfirmRequestArticleCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("ConfirmRequestArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Confirm Request Article")
            .WithDescription("Confirm Request Article")
            .RequireAuthorization();
        
        
        group.MapDelete("/", async ([FromBody] DeleteArticleRequestDto payload,ISender sender) =>
            {
               
                var result = await sender.Send(new DeleteArticleCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("DeleteArticle")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Article")
            .WithDescription("Delete Article")
            .RequireAuthorization();
    }
}