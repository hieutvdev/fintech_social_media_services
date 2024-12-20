using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;
using Auth.Domain.Models;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShredKernel.BaseClasses.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public JwtTokenService(IOptions<JwtConfiguration> jwtConfiguration, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _jwtConfiguration = jwtConfiguration.Value;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateAccessToken(ApplicationUser user, IEnumerable<string>? roles)
    {

        
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
        var singingKey = new SymmetricSecurityKey(key);


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("AvatarUrl", user.AvatarUrl),
            new Claim("BirthDay", user.BirthDay)
        };


        if (roles is not null && roles.Any())
        { 
            claims.AddRange(roles!.Select(r => new Claim(ClaimTypes.Role, r.ToString())));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddDays(30)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes($"{_jwtConfiguration.Secret}{userId}");
        var singingKey = new SymmetricSecurityKey(key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public UserDto GetUserFromClaimToken()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null) {
            throw new BadRequestException("User info is null");
        }

        return new UserDto(
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
        var key = Encoding.UTF8.GetBytes(token);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
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

    public UserDto DecodeToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid token");
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
        
        return new UserDto(
            Id: principal?.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? "",
            UserName: principal?.FindFirst(ClaimTypes.Name)!.Value ?? "",
            FullName: principal?.FindFirst("FullName")!.Value ?? "",
            AvatarUrl: principal?.FindFirst("AvatarUrl")!.Value ?? "",
            BirthDay: principal?.FindFirst("BirthDay")!.Value ?? ""
        );
    }
}