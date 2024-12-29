using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/v1/sso")]
    public class SsoLoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public SsoLoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

       
        [HttpGet("login/{provider}")]
        public async Task<IActionResult> Login(string provider)
        {
            var redirectUrl = Url.Action(nameof(AuthCallback));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }
        
        [HttpGet("auth/callback")]
        public async Task<IActionResult> AuthCallback()
        {
            // var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // if (!authenticateResult.Succeeded)
            // {
            //     return BadRequest("Invalid token");
            // }
            //
            // var claims = authenticateResult.Principal?.Claims.ToDictionary(c => c.Type, c => c.Value);
            // return Ok(new
            // {
            //     Message = "Login successful",
            //     User = claims
            // });
            
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return BadRequest("Invalid token");
            }

            // Lấy thông tin từ Claims
            var claims = authenticateResult.Principal?.Claims;
            var username = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var avatar = claims?.FirstOrDefault(c => c.Type == "urn:discord:avatar")?.Value;

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    Username = username,
                    Email = email,
                    AvatarUrl = !string.IsNullOrEmpty(avatar) 
                        ? $"https://cdn.discordapp.com/avatars/{claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}/{avatar}.png" 
                        : null
                }
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfiguration:Issuer"],
                audience: _configuration["JwtConfiguration:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        
        [HttpGet("login-dc")]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Discord");
        }

        [HttpGet("signin-discord")]
        public IActionResult SignInDiscord()
        {
            return Ok("Logged in with Discord!");
        }
    }
}