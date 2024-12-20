using Article.Application.DTOs.Request.Category;
using Article.Application.UseCases.V1.Commands.Categories.Create;
using Article.Application.UseCases.V1.Commands.Categories.Delete;
using Article.Application.UseCases.V1.Commands.Categories.Update;
using Article.Application.UseCases.V1.Queries.Categories.Detail;
using Article.Application.UseCases.V1.Queries.Categories.GetAll;
using Article.Application.UseCases.V1.Queries.Categories.GetList;
using Article.Application.UseCases.V1.Queries.Categories.GetPage;
using Article.Application.UseCases.V1.Queries.Categories.GetSelect;
using BuildingBlocks.Pagination;
using BuildingBlocks.Utilities.ParamExtensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShredKernel.BaseClasses;


namespace Article.API.Endpoints;

public class CategoryEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/categories";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("categories")
            .MapGroup(BaseUrl).HasApiVersion(1);


        group.MapPost("/", async (CreateCategoryRequestDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateCategoryCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
             .WithName("CreateCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create Category")
            .WithDescription("Create Category")
            .RequireAuthorization();
        
        group.MapGet("/", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllCategoryQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetAllCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetAll Category")
            .WithDescription("GetAll Category")
            .RequireAuthorization();
        
        group.MapGet("/get-select", async (ISender sender) =>
            {
                var result = await sender.Send(new GetSelectCategoryQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetSelectCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Select Category")
            .WithDescription("Get Select Category")
            .RequireAuthorization();
        
        group.MapGet("/get-page", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetPageCategoryQuery(paginationRequest));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetPageCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Page Category")
            .WithDescription("Get Page Category")
            .RequireAuthorization();
        
        
        group.MapGet("/get-list", async ([AsParameters] SearchListModel searchListModel, ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(searchListModel);
                var getListCategoryQuery = new GetListCategoryQuery(paginationRequest, searchBaseModel);
                var result = await sender.Send(getListCategoryQuery);
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetListCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get List Category")
            .WithDescription("Get List Category")
            .RequireAuthorization();
        
        
        group.MapGet("/{id}", async (string id,ISender sender) =>
            {
                var result = await sender.Send(new GetDetailCategoryQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetDetailCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Detail Category")
            .WithDescription("Get Detail Category")
            .RequireAuthorization();
        
        
        group.MapPut("/", async (ISender sender,[FromBody] UpdateCategoryRequestDto requestDto) =>
            {
                var result = await sender.Send(new UpdateCategoryCommand(requestDto));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("UpdateCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Category")
            .WithDescription("Update Category")
            .RequireAuthorization();
        
        group.MapDelete("/", async (ISender sender,[FromBody] DeleteCategoryRequestDto requestDto) =>
            {
                var result = await sender.Send(new DeleteCategoryCommand(requestDto.ids));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("DeleteCategory")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Category")
            .WithDescription("Delete Category")
            .RequireAuthorization();
    }
}