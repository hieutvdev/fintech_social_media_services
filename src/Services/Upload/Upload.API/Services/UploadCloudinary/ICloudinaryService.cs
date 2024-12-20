using Upload.API.Dtos.Responses;

namespace Upload.API.Services.UploadCloudinary;

public interface ICloudinaryService
{
    Task<FileResponseDto> UploadFileCloudinaryAsync(Stream? fileStream, CancellationToken cancellationToken = default!);
    Task<IEnumerable<FileResponseDto>> UploadMultipleFileCloudinaryAsync(IReadOnlyList<IFormFile> fileStreams,  CancellationToken cancellationToken = default!);
    Task<bool> DeleteFileAsync(string publicId, CancellationToken cancellationToken = default!);
}