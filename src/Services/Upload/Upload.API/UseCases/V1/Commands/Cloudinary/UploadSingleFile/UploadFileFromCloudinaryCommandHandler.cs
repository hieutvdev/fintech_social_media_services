using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Upload.API.Dtos.Responses;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.UseCases.V1.Commands.Cloudinary.UploadSingleFile;

public class UploadFileFromCloudinaryCommandHandler
    (ICloudinaryService cloudinaryService)
    : ICommandHandler<UploadFileFromCloudinaryCommand, FileResponseDto>
{
    public async Task<ResultT<FileResponseDto>> Handle(UploadFileFromCloudinaryCommand request, CancellationToken cancellationToken)
    {
        var result = await cloudinaryService.UploadFileCloudinaryAsync(request.Stream, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}