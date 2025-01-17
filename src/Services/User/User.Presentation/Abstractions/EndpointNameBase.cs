namespace User.Presentation.Abstractions;

public static class EndpointNameBase
{
    public static readonly string FriendRequest = "/api/v{version:apiVersion}/friend-request";
    
    public static class Block
    {
        public static readonly string BaseUrl = "/api/v{version:apiVersion}/block";
        public static readonly string Name = BaseUrl.Split("/")[^1];

    }
    
}