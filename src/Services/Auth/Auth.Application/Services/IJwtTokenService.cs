using Auth.Application.DTOs.Response.Auth;
using Auth.Domain.Models;

namespace Auth.Application.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user, IEnumerable<string>? roles);
    string GenerateRefreshToken(string userId);
    UserDto GetUserFromClaimToken();
    bool ValidateToken(string token);
    UserDto DecodeToken(string token);
    
}