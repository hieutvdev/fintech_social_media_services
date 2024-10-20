using BuildingBlocks.CQRS.Commands;
using Upload.API.Dtos.Responses;

namespace Upload.API.UseCases.V1.Commands.Cloudinary.UploadSingleFile;

public record UploadFileFromCloudinaryCommand(Stream Stream) : ICommand<FileResponseDto>;
