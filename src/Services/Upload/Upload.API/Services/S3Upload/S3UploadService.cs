using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Upload.API.Utils;

namespace Upload.API.Services.S3Upload;

public class S3UploadService : IS3UploadService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string? _bucketName;
    private readonly string? _cloudFrontUrl;
    private readonly string? _cloudFrontKeyPairId;
    private readonly string? _privateKey;

    public S3UploadService(IConfiguration configuration, IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
        _bucketName = configuration["S3:BucketName"];
        _cloudFrontUrl = configuration["CloudFront:Url"];
        _cloudFrontKeyPairId = configuration["CloudFront:KeyPairId"];
        _privateKey = configuration["CloudFront:PrivateKey"];
    }
    
    
    
    public async Task<string> S3UploadFileAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        var fileTransferUtility = new TransferUtility(_amazonS3);
        var fileName = FileUtils.GenerateFileName();
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _bucketName
        };
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);
        var url = await RefreshExpireImageUrl(fileName);
        return url;
    }

    public Task<string> S3UploadGenerateCloudFrontAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> S3GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow
        };

        var url = await _amazonS3.GetPreSignedURLAsync(urlRequest);
        return url;
    }

    public async Task<bool> S3DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            var response = await _amazonS3.DeleteObjectAsync(deleteObjectRequest, cancellationToken);
            return response.HttpStatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception e)
        {
            return false;
            throw;
        }
    }


    private async Task<string> RefreshExpireImageUrl(string fileName)
    {
        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(2)
        };
        var signUrl = await _amazonS3.GetPreSignedURLAsync(urlRequest);
        return signUrl;
    }
}