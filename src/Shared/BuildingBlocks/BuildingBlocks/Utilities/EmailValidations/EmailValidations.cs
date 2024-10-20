namespace BuildingBlocks.Utilities.EmailValidations;

public static class EmailValidations
{
    public static bool IsGoogleEmail(string email) => ValidateEmail(email, EmailExtension.GoogleEmailExtensions);

    public static bool IsMicrosoftEmail(string email) => ValidateEmail(email, EmailExtension.MicrosoftEmailExtensions);

    public static bool IsYahooEmail(string email) => ValidateEmail(email, EmailExtension.YahooEmailExtensions);

    public static bool IsAppleEmail(string email) => ValidateEmail(email, EmailExtension.AppleEmailExtensions);

    private static bool ValidateEmail(string email, List<string> emailExtensions)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var emailDomain = email.Split('@').LastOrDefault();
        return emailExtensions.Contains(emailDomain?.ToLower()!);
    }

    public static bool IsEmail(string email) => IsGoogleEmail(email) || IsMicrosoftEmail(email) ||
                                                 IsYahooEmail(email) || IsAppleEmail(email);

}