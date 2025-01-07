namespace BuildingBlocks.Messaging.MessageModels.AuthService;

public record AuthRegisterRequestDto(string UserId, string FullName, string AvatarUrl, string BirthDay, int Gender);