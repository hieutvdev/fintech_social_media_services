using BuildingBlocks.Repository.ClientCallApi;
using BuildingBlocks.Security;
using Serilog;
using User.Application.DTOs.Response.AuthService;
using User.Application.Services;

namespace User.Infrastructure.Services;

public class AuthServerService(IClientCallApi clientCallApi, IAuthorizeExtension authorizeExtension) : IAuthServerService
{
    public async Task<IEnumerable<UserShareResDto>> GetUserShareAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        if (ids == null || !ids.Any())
        {
            throw new ArgumentException("Ids cannot be null or empty.", nameof(ids));
        }

        var queryString = string.Join("&", ids.Select(id => $"ids={Uri.EscapeDataString(id)}"));
        var requestUrl = $"https://localhost:7002/api/v1/auth/user-share?{queryString}";
        
        var token = authorizeExtension.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Authorization token is missing.");
        }
        
        var headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer " + token }
        };
        

        try
        {
            
            var response = await clientCallApi.GetAsync<IEnumerable<UserShareResDto>>(requestUrl, headers, cancellationToken);
            return response ?? Enumerable.Empty<UserShareResDto>();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to fetch user shares from the API.", ex);
        }
    }

}