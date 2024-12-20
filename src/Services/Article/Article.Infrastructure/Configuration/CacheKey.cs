namespace Article.Infrastructure.Configuration;

public class CacheKey
{
    public const string Domain = "FSM-Article-";

    public static class Article
    {
        public static readonly string ArticleEntity = $"{Domain}AE-";
    }

    public static class Category
    {
        public static readonly string CategoryEntity = $"{Domain}CA-";
    }
    
    public static class Tag
    {
        public static readonly string TagEntity = $"{Domain}TA-";
    }
}