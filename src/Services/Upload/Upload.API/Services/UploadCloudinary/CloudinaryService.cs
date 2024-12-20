using System.Net;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Utilities.FileValidations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Upload.API.Configurations;
using Upload.API.Dtos.Responses;
using Upload.API.Exceptions;
using Upload.API.Utils;

namespace Upload.API.Services.UploadCloudinary;

public class CloudinaryService : ICloudinaryService
{

    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinaryConfigurationSetting> options)
    {
        var account = new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }


    private string CreateImageUrl(string publicId, int width, int height, bool highRes)
    {
        var transformation = highRes
            ? new Transformation().Width(width).Height(height).Crop("fill")
            : new Transformation().Width(width).Height(height).Crop("fill").Quality(50);

        return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
    }
    public async Task<FileResponseDto> UploadFileCloudinaryAsync(Stream? fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new BadRequestException("File is missing");
        }

        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        try
        {
            var fileName = FileUtils.GenerateFileName();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error is not null)
            {
                throw new FileUploadException("Cloudinary Error Upload");
            }
            
            var smallUrl = CreateImageUrl(uploadResult.PublicId, uploadResult.Width / 8, uploadResult.Height / 8, true);

            return new FileResponseDto(uploadResult.Url.ToString(), smallUrl, fileName, "CL");
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<FileResponseDto>> UploadMultipleFileCloudinaryAsync(IReadOnlyList<IFormFile> fileStreams, CancellationToken cancellationToken = default)
    {
        var uploadFiles = new List<FileResponseDto>();

        foreach (var file in fileStreams)
        { 
            var defaultFileName = file.FileName;

            var fileName = FileUtils.GenerateFileName();

            var fileType = FileValidations.GetFileType(defaultFileName);
        
            if (!FileValidations.ValidationFileStreamSize(fileType, file.OpenReadStream()))
            {
                continue;
            }
            
           
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream)
                };
                var upload = await Task.Run(() => _cloudinary.UploadAsync(uploadParams), cancellationToken);
                uploadFiles.Add(new FileResponseDto(upload.Url.ToString(), CreateImageUrl(upload.PublicId, upload.Width /8, upload.Height/8, true), fileName, "CL"));
            }
            
        }
        return uploadFiles;
    }

    public async Task<bool> DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = publicId.Split("/");
            var id = url[^1].Split(".")[0];
            if (string.IsNullOrEmpty(id))
                throw new BadRequestException("publicIc cannot be null or empty");
            
            var deleteParams = new DeletionParams(id);
            var result = await Task.Run(() => _cloudinary.DestroyAsync(deleteParams), cancellationToken);

            return result.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            throw new FileUploadException(e.Message);
        }
    }
}