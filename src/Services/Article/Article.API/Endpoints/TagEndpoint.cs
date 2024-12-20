using Article.Application.DTOs.Request.Tag;
using Article.Application.UseCases.V1.Commands.Tags.Create;
using Article.Application.UseCases.V1.Commands.Tags.Delete;
using Article.Application.UseCases.V1.Commands.Tags.Update;
using Article.Application.UseCases.V1.Queries.Tags.Detail;
using Article.Application.UseCases.V1.Queries.Tags.GetAll;
using Article.Application.UseCases.V1.Queries.Tags.GetList;
using Article.Application.UseCases.V1.Queries.Tags.GetPage;
using Article.Application.UseCases.V1.Queries.Tags.GetSelect;
using BuildingBlocks.Pagination;
using BuildingBlocks.Utilities.ParamExtensions;
using Microsoft.AspNetCore.Mvc;
using ShredKernel.BaseClasses;

namespace Article.API.Endpoints;

public class TagEndpoint : ICarterModule
{
    
    private const string BaseUrl = "/api/v{version:apiVersion}/tags";
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        var group = app.NewVersionedApi("tags")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group.MapPost("/" , async ([FromBody] CreateTagRequestDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateTagCommand(payload));
                var response = result;
                Console.WriteLine(response);
                return !response.IsSuccess ? Results.BadRequest(response) : Results.Ok(result);
            }) 
            .WithName("CreateTag")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create Tag")
            .WithDescription("Create Tag")
            .RequireAuthorization();
        
        
        group.MapGet("/", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllTagQuery());
                return !result.IsSuccess ? Results.BadRequest() : Results.Ok(result);
            })
            .WithName("GetAllTag")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get All Tag")
            .WithDescription("Get All Tag")
            .RequireAuthorization();
        
        
        group.MapGet("/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetDetailTagQuery(id));
                return !result.IsSuccess ? Results.BadRequest() : Results.Ok(result);
            })
            .WithName("GetDetailTag")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Details")
            .WithDescription("Get Details")
            .RequireAuthorization();
        
        
        group.MapGet("/get-page", async ([AsParameters] PaginationRequest query, ISender sender) =>
            {
                var result = await sender.Send(new GetPageTagQuery(query));
                return !result.IsSuccess ? Results.BadRequest() : Results.Ok(result);
            })
            .WithName("GetPageTags")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Page")
            .WithDescription("Get Page")
            .RequireAuthorization();
        
        group.MapGet("/get-select", async (ISender sender) =>
            {
                var result = await sender.Send(new GetSelectTagQuery());
                return !result.IsSuccess ? Results.BadRequest() : Results.Ok(result);
            })
            .WithName("GetSelectTags")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Select")
            .WithDescription("Get Select")
            .RequireAuthorization();
        
        group.MapGet("/get-list", async ([AsParameters] SearchListModel query, ISender sender) =>
            {
                var (searchBaseModel, paginationRequest) = ParamConvert.ParamConvertSearchModelList(query);
                var result = await sender.Send(new GetListTagQuery(paginationRequest, searchBaseModel));
                return !result.IsSuccess ? Results.BadRequest() : Results.Ok(result);
            })
            .WithName("GetListTags")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get List")
            .WithDescription("Get List")
            .RequireAuthorization();
        
        
        group.MapPut("/" , async ([FromBody] UpdateTagRequestDto payload, ISender sender) =>
            {
                var result = await sender.Send(new UpdateTagCommand(payload));
                var response = result;
                return !response.IsSuccess ? Results.BadRequest(response) : Results.Ok(result);
                
            }) 
            .WithName("UpdateTag")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Tag")
            .WithDescription("Update Tag")
            .RequireAuthorization();
        
        
        group.MapDelete("/" , async ([FromBody] DeleteTagRequestDto payload, ISender sender) =>
            {
                var result = await sender.Send(new DeleteTagCommand(payload));
                var response = result;
                return !response.IsSuccess ? Results.BadRequest(response) : Results.Ok(result);
                
            }) 
            .WithName("DeleteTag")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Tag")
            .WithDescription("Delete Tag")
            .RequireAuthorization();
    }
}