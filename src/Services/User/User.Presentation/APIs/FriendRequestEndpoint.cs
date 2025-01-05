
using User.Application.DTOs.Request.FriendRequest;
using User.Application.UseCases.Commands.FriendRequest.Create;
using User.Application.UseCases.Commands.FriendRequest.Delete;
using User.Application.UseCases.Queries.FriendRequest.GetListByReceiver;
using User.Application.UseCases.Queries.FriendRequest.GetListBySender;


namespace User.Presentation.APIs;

public class FriendRequestEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/friend-request";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("friend-request")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group.MapPost("/", async ([FromBody] CreateFriendRequestReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateFriendRequestCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("CreateFriendRequest")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create FriendRequest")
            .WithDescription("Create FriendRequest")
            .RequireAuthorization();
        
        group.MapGet("/by-sender", async ([AsParameters] PaginationRequest query ,ISender sender) =>
            {
                var result = await sender.Send(new GetListBySenderFriendRequestQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetListBySenderFriendRequest")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetListBySender FriendRequest")
            .WithDescription("GetListBySender FriendRequest")
            .RequireAuthorization();
        
        
        group.MapGet("/by-receiver", async ([AsParameters] PaginationRequest query ,ISender sender) =>
            {
                var result = await sender.Send(new GetListByReceiverFriendRequestQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetListByReceiverFriendRequest")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetListByReceiver FriendRequest")
            .WithDescription("GetListByReceiver FriendRequest")
            .RequireAuthorization();
        
        
        group.MapDelete("/", async ([FromBody] DeleteFriendRequestReqDto payload ,ISender sender) =>
            {
                var result = await sender.Send(new DeleteFriendRequestCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("DeleteFriendRequest")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete FriendRequest")
            .WithDescription("Delete FriendRequest")
            .RequireAuthorization();
    }
}