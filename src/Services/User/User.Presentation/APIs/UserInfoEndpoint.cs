
using User.Application.DTOs.Request.UserInfo;
using User.Application.UseCases.Commands.UserInfo.Create;
using User.Application.UseCases.Commands.UserInfo.Update;
using User.Application.UseCases.Queries.UserInfo.Detail;

namespace User.Presentation.APIs;

public class UserInfoEndpoint: ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/user-info";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("user-info")
            .MapGroup(BaseUrl).HasApiVersion(1);


        group.MapPost("/", async ([FromBody] CreateUserInfoReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new CreateUserInfoCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            }).WithName("CreateUserInfo")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create UserInfo")
            .WithDescription("Create UserInfo");
            
        
        
        group.MapGet("/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserInfoDetailQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("GetDetailUserInfo")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetDetail UserInfo")
            .WithDescription("GetDetail UserInfo")
            .RequireAuthorization();
     
        group.MapPut("/", async ([FromBody] UpdateUserInfoReqDto payload, ISender sender) =>
            {
                var result = await sender.Send(new UpdateUserInfoCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("UpdateUserInfo")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update UserInfo")
            .WithDescription("Update UserInfo")
            .RequireAuthorization();
        

    }
}