using User.Application.DTOs.Request.UserType;
using User.Application.UseCases.Commands.UserType.Create;
using User.Application.UseCases.Commands.UserType.Delete;
using User.Application.UseCases.Commands.UserType.Update;
using User.Application.UseCases.Models.UserType;
using User.Application.UseCases.Queries.UserType.Detail;
using User.Application.UseCases.Queries.UserType.GetAll;
using User.Application.UseCases.Queries.UserType.GetList;
using User.Application.UseCases.Queries.UserType.GetSelect;

namespace User.Presentation.APIs;

public class UserTypeEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/user-type";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("user-type")
            .MapGroup(BaseUrl).HasApiVersion(1);
        

        group.MapPost("/", async ([FromBody] CreateUserTypeReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateUserTypeCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("CreateUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create UserType")
            .WithDescription("Create UserType")
            .RequireAuthorization();
        
        group.MapGet("/", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllUserTypeQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetAllUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetAll UserType")
            .WithDescription("GetAll UserType")
            .RequireAuthorization();
        
        group.MapGet("/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetDetailUserTypeQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("DetailUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Detail UserType")
            .WithDescription("Detail UserType")
            .RequireAuthorization();
        
        group.MapGet("/get-list", async ([AsParameters] UserTypeSearchListModelQuery query, ISender sender) =>
            {
                var result = await sender.Send(new GetListUserTypeQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetListUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetList UserType")
            .WithDescription("GetList UserType")
            .RequireAuthorization();
        
        
        group.MapGet("/get-select", async (ISender sender) =>
            {
                var result = await sender.Send(new GetSelectUserTypeQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetSelectUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetSelect UserType")
            .WithDescription("GetSelect UserType")
            .RequireAuthorization();
        
        
        group.MapPut("/", async ([FromBody] UpdateUserTypeReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new UpdateUserTypeCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("UpdateUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update UserType")
            .WithDescription("Update UserType")
            .RequireAuthorization();
        
        
        group.MapDelete("/", async ([FromBody] DeleteUserTypeReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new DeleteUserTypeCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("DeleteUserType")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete UserType")
            .WithDescription("Delete UserType")
            .RequireAuthorization();

    }
}