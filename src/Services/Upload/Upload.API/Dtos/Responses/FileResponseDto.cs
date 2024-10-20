namespace Upload.API.Dtos.Responses;

public record FileResponseDto(string FullSize, string SmallSize, string Key, string FileType);