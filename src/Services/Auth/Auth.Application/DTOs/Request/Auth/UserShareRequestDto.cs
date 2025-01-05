namespace Auth.Application.DTOs.Request.Auth;

public record UserShareRequestDto(IEnumerable<string> Ids);