using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Upload.API.Dtos.Responses;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.UseCases.V1.Commands.Cloudinary.UploadMultipleFile;

public class UploadMultipleFileFromCloudinaryCommandHandler(ICloudinaryService service)
    : ICommandHandler<UploadMultipleFileFromCloudinaryCommand, IEnumerable<FileResponseDto>>
{
    public async Task<ResultT<IEnumerable<FileResponseDto>>> Handle(UploadMultipleFileFromCloudinaryCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UploadMultipleFileCloudinaryAsync(request.Files, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}