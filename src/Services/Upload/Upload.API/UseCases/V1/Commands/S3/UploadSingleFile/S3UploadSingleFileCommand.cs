using BuildingBlocks.CQRS.Commands;
using Upload.API.Dtos.Responses;

namespace Upload.API.UseCases.V1.Commands.S3.UploadSingleFile;

public record S3UploadSingleFileCommand(Stream Stream) : ICommand<FileResponseDto>;