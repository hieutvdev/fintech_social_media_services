namespace Auth.Infrastructure.Configuration;

public static class CacheKey
{
    public const string Domain = "FSM-Auth-";
    public static class Auth
    {
        public static readonly string RefreshToken = "RT-";
        public static readonly string AccessToken = "AT-";
        public static readonly string ForgotPassword = "FP-";
        public static readonly string ConfirmEmail = "CF-";
        public static readonly string VerifyCode = "VF-";
        public static readonly string VerifyLoginCode = "VLC-";
    }
}