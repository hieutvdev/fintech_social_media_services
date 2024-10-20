namespace Upload.API.Utils;

public static class FileUtils
{
    public static string GenerateFileName()
    {
        return $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}