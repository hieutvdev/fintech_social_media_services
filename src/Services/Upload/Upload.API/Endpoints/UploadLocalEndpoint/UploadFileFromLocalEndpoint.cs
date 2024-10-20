using BuildingBlocks.CQRS.Common;
using Carter;
using MediatR;
using Upload.API.UseCases.V1.Commands.Upload.UploadFileLocal;

namespace Upload.API.Endpoints.UploadLocalEndpoint;

public class UploadFileFromLocalEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/upload";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("upload")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group.MapPost("/locals/single-file", async (HttpContext context, ISender sender) =>
        {
            var result = await sender.Send(new UploadFileLocalCommand(context));
            return Results.Ok(result);
        });

    }
}