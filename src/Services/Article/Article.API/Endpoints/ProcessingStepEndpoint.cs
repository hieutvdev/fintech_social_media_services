using Article.Application.DTOs.Request.ProcessingStep;
using Article.Application.UseCases.V1.Commands.ProcessingStep.Create;
using Article.Application.UseCases.V1.Commands.ProcessingStep.Update;
using Article.Application.UseCases.V1.Queries.ProcessingStep.Detail;
using Article.Application.UseCases.V1.Queries.ProcessingStep.GetAll;
using Article.Application.UseCases.V1.Queries.ProcessingStep.GetByUser;
using Article.Application.UseCases.V1.Queries.ProcessingStep.GetList;

namespace Article.API.Endpoints;

public class ProcessingStepEndpoint: ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/processing-step";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("processing-step")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group.MapPost("/", async (CreateProcessingRequestDto model, ISender sender) =>
            {
                var result = await sender.Send(new CreateProcessingStepCommand(model));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("CreateProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create ProcessingStep")
            .WithDescription("Create ProcessingStep")
            .RequireAuthorization();
        
        
        group.MapGet("/get-detail/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetDetailProcessingStepQuery(id));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })  .WithName("DetailProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Detail ProcessingStep")
            .WithDescription("Detail ProcessingStep")
            .RequireAuthorization();
        
        
        group.MapGet("/get-list", async ([AsParameters] ProcessingStepSearchListDto query,ISender sender) =>
            {
               
                var result = await sender.Send(new GetListProcessingStepQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetListProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetList ProcessingStep")
            .WithDescription("GetList ProcessingStep")
            .RequireAuthorization();
        
        group.MapGet("/get-list-by-user", async ([AsParameters] ProcessingStepSearchListDto query,ISender sender) =>
            {
               
                var result = await sender.Send(new GetByUserProcessingStepQuery(query));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetListByUserProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetListByUser ProcessingStep")
            .WithDescription("GetListByUser ProcessingStep")
            .RequireAuthorization();
        
        
        group.MapGet("/" , async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllProcessingStepQuery());
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("GetAllProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetAll ProcessingStep")
            .WithDescription("GetAll ProcessingStep")
            .RequireAuthorization();
        
        
        group.MapPut("/", async (UpdateProcessingRequestDto payload, ISender sender) =>
            {
                var result = await sender.Send(new UpdateProcessingStepCommand(payload));
                var response = result;
                return response.IsFailure ? Results.BadRequest(response) : Results.Ok(response);
            })
            .WithName("UpdateProcessingStep")
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update ProcessingStep")
            .WithDescription("Update ProcessingStep")
            .RequireAuthorization();
    }
}