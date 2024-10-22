using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShredKernel.BaseClasses;
using ShredKernel.BaseClasses.Configurations;

namespace BuildingBlocks.Security;

public class AuthorizeExtension : IAuthorizeExtension
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthorizeExtension(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfiguration> options)
    {
        this._jwtConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    public UserLoginResponseBase GetUserFromClaimToken()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }

        return new UserLoginResponseBase
        (
            Id: user?.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? "",
            UserName: user?.FindFirst(ClaimTypes.Name)!.Value ?? "",
            FullName: user?.FindFirst("FullName")!.Value ?? "",
            AvatarUrl: user?.FindFirst("AvatarUrl")!.Value ?? "",
            BirthDay: user?.FindFirst("BirthDay")!.Value ?? ""
        );
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(this._jwtConfiguration.Secret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = this._jwtConfiguration.Issuer,
            ValidAudience = this._jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public UserLoginResponseBase DecodeToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid Token");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(token))
        {
            throw new SecurityTokenException("Invalid token format");
        }
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
        var singingKey = new SymmetricSecurityKey(key);
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
            IssuerSigningKey = singingKey
        };
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        
        return new UserLoginResponseBase(
            Id: principal?.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? "",
            UserName: principal?.FindFirst(ClaimTypes.Name)!.Value ?? "",
            FullName: principal?.FindFirst("FullName")!.Value ?? "",
            AvatarUrl: principal?.FindFirst("AvatarUrl")!.Value ?? "",
            BirthDay: principal?.FindFirst("BirthDay")!.Value ?? ""
        );
    }
}