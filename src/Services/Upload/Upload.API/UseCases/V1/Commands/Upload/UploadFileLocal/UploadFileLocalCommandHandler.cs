using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Upload.API.Dtos.Responses;
using Upload.API.Exceptions;
using Upload.API.Services.Upload;

namespace Upload.API.UseCases.V1.Commands.Upload.UploadFileLocal;

public class UploadFileLocalCommandHandler
(IUploadService uploadService)
: ICommandHandler<UploadFileLocalCommand, FileResponseDto>
{
    public async Task<ResultT<FileResponseDto>> Handle(UploadFileLocalCommand request, CancellationToken cancellationToken)
    {
        var fromCollection = request.Context.Request.ReadFormAsync(cancellationToken);
        var file = fromCollection.Result.Files.GetFile("file");
        if (file is null)
        {
            throw new FileUploadException("Upload file local failure");
        }
        var result = await uploadService.UploadFileAsync(file, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}