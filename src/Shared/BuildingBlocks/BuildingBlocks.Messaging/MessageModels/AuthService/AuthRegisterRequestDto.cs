namespace BuildingBlocks.Messaging.MessageModels.AuthService;

#nullable disable
public class AuthRegisterRequestDto
{
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public int BirthDay { get; set; }
    public int Gender { get; set; }
}

