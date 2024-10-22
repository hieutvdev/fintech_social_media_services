using Article.Application.DTOs.Request.Category;
using Article.Application.UseCases.V1.Commands.Categories.Create;
using Article.Application.UseCases.V1.Queries.Categories.GetAll;


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
        .RequireAuthorization();;
    }
}