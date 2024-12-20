using BuildingBlocks.CQRS.Commands;
using Upload.API.Dtos.Responses;

namespace Upload.API.UseCases.V1.Commands.Cloudinary.UploadMultipleFile;

public record UploadMultipleFileFromCloudinaryCommand(IReadOnlyList<IFormFile> Files) : ICommand<IEnumerable<FileResponseDto>>;