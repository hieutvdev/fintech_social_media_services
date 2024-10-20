using BuildingBlocks.CQRS.Common;
using Carter;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using Upload.API.UseCases.V1.Commands.S3.UploadSingleFile;

namespace Upload.API.Endpoints.UploadS3Endpoint;

public class UploadS3Endpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/upload";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("upload")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group.MapPost("/s3/single-file", async (HttpRequest request, ISender sender) =>
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
            {
                return Results.BadRequest(new Error("404", "File missing"));
            }

            var file = request.Form.Files[0];
            await using var stream = file.OpenReadStream();
            var result = await sender.Send(new S3UploadSingleFileCommand(stream));
            return Results.Ok(result);
        });
    }
}