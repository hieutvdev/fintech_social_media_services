using BuildingBlocks.Exceptions;
using BuildingBlocks.Utilities.FileValidations;
using Upload.API.Dtos.Responses;
using Upload.API.Exceptions;
using Upload.API.Utils;

namespace Upload.API.Services.Upload;

public class UploadService : IUploadService
{
    private readonly string _domain = "http://localhost:5239/";
    private readonly string _targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FileUploads");
    
    
    public async Task<FileResponseDto> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        try
        {
            if (file == null || file.Length < 0)
                throw new FileUploadException("File cannot null");
            if (!FileValidations.ValidationFileSize("Image", file))
                throw new BadRequestException("File size exceeds the 1 MB limits");
            if(!FileValidations.IsImage(Path.GetExtension(file.FileName).ToLowerInvariant()))
                throw new BadRequestException("Unsupported file type");

            string newFileName = FileUtils.GenerateFileName() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_targetFilePath, newFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            return new FileResponseDto($"{_domain}file-uploads/{newFileName}", "", newFileName, "LC");
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
    
    private string GetFilePathFromFileUrl(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            throw new BadRequestException("FileUrl is null");
        if (!fileUrl.Contains("/"))
            throw new BadRequestException("FileUrl invalid");
        string filePath = Path.Combine(_targetFilePath, fileUrl.Split("/")[^1]);
        return filePath ?? "";
    }

    public async Task<IEnumerable<FileResponseDto>> UploadMultipleFileAsync(IReadOnlyList<IFormFile> files, CancellationToken cancellationToken = default)
    {
        try
        {
            List<FileResponseDto> fileResponseDtos = new List<FileResponseDto>();
            foreach (var file in files)
            {
                var fileResult = await UploadFileAsync(file, cancellationToken);
                if (!string.IsNullOrEmpty(fileResult.FullSize))
                {
                    throw new BadRequestException("Upload file failure");
                }
                fileResponseDtos.Add(fileResult);
            }
            if(fileResponseDtos is null)
                throw new BadRequestException("Upload file failure");

            return fileResponseDtos;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
    
    private bool FileExists(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return false;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool DeleteMultipleFile(IEnumerable<string> fileUrls)
    {
        var enumerable = fileUrls as string[] ?? fileUrls.ToArray();
        if (!enumerable.Any())
        {
            throw new BadRequestException("There ís no path any file");
        }
        try
        {
            foreach (var fileUrl in enumerable)
            {
                if (string.IsNullOrEmpty(GetFilePathFromFileUrl(fileUrl)) || FileExists(fileUrl))
                {
                    File.Delete(fileUrl);
                    continue;
                }
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}