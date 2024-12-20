using BuildingBlocks.CQRS.Common;
using Carter;
using MediatR;
using Upload.API.UseCases.V1.Commands.Cloudinary.UploadMultipleFile;
using Upload.API.UseCases.V1.Commands.Cloudinary.UploadSingleFile;

namespace Upload.API.Endpoints.UploadCloudinaryEndpoint;

public class UploadFileCloudinaryEndpoint: ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/upload";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("upload")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group.MapPost("/cld/single-file", async (HttpRequest request, ISender sender) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                return Results.BadRequest("No file upload!");
            }

            var file = request.Form.Files[0];
            await using var stream = file.OpenReadStream();
            var result = await sender.Send(new UploadFileFromCloudinaryCommand(stream));
            return Results.Ok(result);
        });
        
        
        group.MapPost("/cld/multiple-file", async (HttpContext context, ISender sender) =>
        {
            var fromCollection = await context.Request.ReadFormAsync();
            var files = fromCollection.Files.GetFiles("files");

            if (!files.Any())
            {
                return Results.BadRequest(new Error("404", "File Missing"));
            }
            var result = await sender.Send(new UploadMultipleFileFromCloudinaryCommand(files));
            return Results.Ok(result);
        });

    }
}