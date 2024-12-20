namespace BuildingBlocks.Utilities.EmailValidations;

public static class EmailExtension
{
    public static readonly List<string> GoogleEmailExtensions = new()
    {
        "gmail.com",
        "googlemail.com"
    };
    
    public static readonly List<string> MicrosoftEmailExtensions = new()
    {
        "outlook.com",
        "hotmail.com",
        "live.com",
        "msn.com"
    };

    
    public static readonly List<string> YahooEmailExtensions = new()
    {
        "yahoo.com",
        "ymail.com",
        "rocketmail.com"
    };

    
    public static readonly List<string> AppleEmailExtensions = new()
    {
        "icloud.com",
        "me.com",
        "mac.com"
    };
    
}