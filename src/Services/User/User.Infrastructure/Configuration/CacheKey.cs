namespace User.Infrastructure.Configuration;

public class CacheKey
{
    private const string Domain = "FSM-User";
    

    public static class UserType
    {
        public static readonly string GetAll = $"{Domain}-UserType-GetAll";
        public static readonly string GetSelect = $"{Domain}-UserType-GetSelect";
        public static readonly string GetList = $"{Domain}-UserType-GetList-{{0}}";
        public static readonly string GetDetail = $"{Domain}-UserType-GetDetail-{{0}}";
        public static readonly string DeleteAllCache = $"{Domain}-UserType-";
    }
    
    
    public static class UserInfo
    {
        public static readonly string GetDetail = $"{Domain}-UserInfo-GetDetail-{{0}}";
    }
    
    
    public static class FriendShip
    {
        public static readonly string GetList = $"{Domain}-FriendShip-GetList-{{0}}";
    }
    
}