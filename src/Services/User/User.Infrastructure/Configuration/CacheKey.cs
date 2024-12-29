namespace User.Infrastructure.Configuration;

public class CacheKey
{
    private const string Domain = "FSM-User";
    

    public static class UserType
    {
        public static readonly string GetAll = $"{Domain}-UserType-GetAll";
        public static readonly string GetSelect = $"{Domain}-UserType-GetSelect";
        public static readonly string GetDetail = $"{Domain}-UserType-GetDetail-{0}";
        public static readonly string DeleteAllCache = $"{Domain}-UserType-";
    }

   
    
    
    
}