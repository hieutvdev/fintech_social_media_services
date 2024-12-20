namespace BuildingBlocks.Helpers;

public static class FluentValidationGenContent
{
    public static string GenMessageFieldEmpty(string fieldName)
    {
        return $"{fieldName} cannot be empty or space";
    }
    
    public static string GenMessageObjectEmpty()
    {
        return $"Data payload cannot be empty";
    }
}