using MassTransit;
using User.Application.DTOs.Request.Block;
using User.Application.UseCases.Commands.Block.CheckBlocked;
using User.Application.UseCases.Commands.Block.Create;
using User.Application.UseCases.Commands.Block.Delete;
using User.Application.UseCases.Queries.Block.GetByUserLogin;
using User.Presentation.Abstractions;

namespace User.Presentation.APIs;

public class BlockEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi(EndpointNameBase.Block.Name)
            .MapGroup(EndpointNameBase.Block.BaseUrl).HasApiVersion(1);
        
        
        
        group.MapPost("/", async (CreateBlockReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateBlockCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
        .WithName("CreateBlock")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Block")
        .WithDescription("Create Block")
        .RequireAuthorization();
        
        
        group.MapDelete("/", async (DeleteBlockReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBlockCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
        .WithName("DeleteBlock")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Block")
        .WithDescription("Delete Block")
        .RequireAuthorization();
            
        
        group.MapGet("/check-block/{id}", async (string userId, ISender sender) =>
            {
                var result = await sender.Send(new CheckBlockCommand(userId));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
        .WithName("Check-Block")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Check Block")
        .WithDescription("Check Block")
        .RequireAuthorization();

        
        
        group.MapGet("/get-by-user-login", async (PaginationRequest query, ISender sender) =>
            {
                var result = await sender.Send(new GetBlockByUserLoginQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
        .WithName("GetByUserLoginBlock")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("GetByUserLogin Block")
        .WithDescription("GetByUserLogin Block")
        .RequireAuthorization();
    }
}