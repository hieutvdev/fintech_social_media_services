using Upload.API.Dtos.Responses;

namespace Upload.API.Services.Upload;

public interface IUploadService
{
    Task<FileResponseDto> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default!);
    Task<IEnumerable<FileResponseDto>> UploadMultipleFileAsync(IReadOnlyList<IFormFile> files, CancellationToken cancellationToken = default!);
    bool DeleteMultipleFile(IEnumerable<string> fileUrls);
}