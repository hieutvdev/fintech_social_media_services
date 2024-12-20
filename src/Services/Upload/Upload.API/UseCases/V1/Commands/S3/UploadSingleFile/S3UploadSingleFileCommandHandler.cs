using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Upload.API.Dtos.Responses;
using Upload.API.Services.S3Upload;

namespace Upload.API.UseCases.V1.Commands.S3.UploadSingleFile;

public class S3UploadSingleFileCommandHandler(IS3UploadService service)
: ICommandHandler<S3UploadSingleFileCommand, FileResponseDto>
{
    public async Task<ResultT<FileResponseDto>> Handle(S3UploadSingleFileCommand request, CancellationToken cancellationToken)
    {
        var result = await service.S3UploadFileAsync(request.Stream, cancellationToken);
        FileResponseDto fileResponseDto = new FileResponseDto(result, "", result, "S3");
        var response = Result.Create(value: fileResponseDto);
        return response;
    }
}