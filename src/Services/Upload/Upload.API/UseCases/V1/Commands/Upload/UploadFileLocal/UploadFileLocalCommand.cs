using BuildingBlocks.CQRS.Commands;
using Upload.API.Dtos.Responses;

namespace Upload.API.UseCases.V1.Commands.Upload.UploadFileLocal;

public record UploadFileLocalCommand(HttpContext Context) : ICommand<FileResponseDto>;