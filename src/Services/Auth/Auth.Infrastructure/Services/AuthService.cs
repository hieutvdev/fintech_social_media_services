
using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;
using Auth.Domain.Models;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Auth.Application.DTOs.Request.MailService;
using Auth.Application.Exceptions;
using Auth.Application.Extensions;
using Auth.Infrastructure.Configuration;
using AutoMapper;
using BuildingBlocks.Messaging.Messaging.Kafka;
using BuildingBlocks.Utilities.EmailValidations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using ShredKernel.BaseClasses;

namespace Auth.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ICacheService _cacheService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IKafkaProducerService<string, string> _producer;
    private readonly IMapper _mapper;

    public AuthService(
        ICacheService cacheService,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IJwtTokenService jwtTokenService,
        IKafkaProducerService<string, string> producer,
        IMapper mapper)
    {
        _cacheService = cacheService;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _jwtTokenService = jwtTokenService;
        _producer = producer;
        _mapper = mapper;
    }
    
    
    


    private string GetDeviceInfo()
    {
        var ipAddress = _contextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString();
        var userAgent = _contextAccessor.HttpContext.Request.Headers.UserAgent.ToString();
        string deviceInfor = $"{ipAddress}-{userAgent}";

        while (deviceInfor.Contains(" ")) {
            deviceInfor = deviceInfor.Replace(" ", "");
        }
        return deviceInfor;
    }
    
    
    

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto, CancellationToken cancellationToken = default)
    {
        var user = _jwtTokenService.GetUserFromClaimToken();
        if (string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.UserName))
        {
            throw new BadRequestException("Invalid user");
        }

        var userFound = await _userManager.FindByIdAsync(user.Id);
        if (userFound is null)
        {
            throw new UserNotFoundException($"{userFound!.Id}");
        }

        if (!string.Equals(changePasswordRequestDto.NewPassword, changePasswordRequestDto.ConfirmPassword))
        {
            throw new BadRequestException("Confirm password don't matching");
        }

        var isCheckPassword = await _userManager.CheckPasswordAsync(userFound, changePasswordRequestDto.OldPassword);

        if (!isCheckPassword)
        {
            throw new BadRequestException("Password is incorrect");
        }

        var isChangePasswordSuccess =
            await _userManager.ChangePasswordAsync(userFound,changePasswordRequestDto.OldPassword, changePasswordRequestDto.ConfirmPassword);
        if (!isChangePasswordSuccess.Succeeded)
        {
            return false;
        }
        string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{user.Id}";
        string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{user.Id}";
        await Task.WhenAll(_cacheService.RemoveCacheAsync(accessTokenCacheKey),
            _cacheService.RemoveCacheAsync(refreshTokenCacheKey));

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto,
        CancellationToken cancellationToken = default)
    {
        if (!EmailValidations.IsEmail(forgotPasswordRequestDto.Email))
        {
            throw new BadRequestException("Invalid Email");
        }
        var userFound = await _userManager.FindByEmailAsync(forgotPasswordRequestDto.Email);
        if (userFound is null)
        {
            throw new UserNotFoundException($"{forgotPasswordRequestDto.Email}");
        }

        var cacheKeyForLock = $"{CacheKey.Domain}{CacheKey.Auth.ForgotPassword}Lock-{forgotPasswordRequestDto.Email}";
        var cacheToken = $"{CacheKey.Domain}{CacheKey.Auth.ForgotPassword}{forgotPasswordRequestDto.Email}";
        int requestCount = await _cacheService.GetIncrementValueAsync(cacheKeyForLock);
        TimeSpan? expire = await _cacheService.GetKeyTimeToLiveAsync(cacheKeyForLock);
        if (requestCount == 1)
        {
            await _cacheService.SetExpireForKeyAsync(cacheKeyForLock, TimeSpan.FromMinutes(5));
        }
        
        if (requestCount >= 4)
        {
            throw new BadRequestException($"Account is locker due to too many requests, Try again later {expire.ToString()}");
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(userFound);
        await _cacheService.SetStringIncrementAsync(cacheKeyForLock);
        await _cacheService.SetCacheAsync(cacheToken, token, TimeSpan.FromMinutes(2));

        MailRequestDto mailRequestDto = new MailRequestDto(forgotPasswordRequestDto.Email,
            "Nhấn vào liên kết để cập nhật lại mật khẩu", token);
        string mailSerialization = JsonConvert.SerializeObject(mailRequestDto);
        await _producer.ProduceAsync("send-mail", "forgot-password", mailSerialization);
        return true;
    }

    public async Task<LoginResponseDto> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheTokenConfirmEmailKey =
                $"FSM-Auth-CF-admin@gmail.com";
            var cacheResponse = await _cacheService.GetCacheAsync(cacheTokenConfirmEmailKey);
            //cacheResponse = cacheResponse.Replace("\"", "");
            var tokenResponse = cacheResponse.Substring(1, cacheResponse.Length - 2);
            if (string.IsNullOrEmpty(cacheResponse) || !string.Equals(confirmEmailRequestDto.Token, tokenResponse))
            {
                throw new BadRequestException("Invalid token");
            }

            var user = await _userManager.FindByEmailAsync(confirmEmailRequestDto.Email);
            if (user is null)
            {
                throw new UserNotFoundException($"{confirmEmailRequestDto.Email}");
            }

            var isConfirmEmail = await _userManager.ConfirmEmailAsync(user, confirmEmailRequestDto.Token);
            if (!isConfirmEmail.Succeeded)
            {
                throw new BadRequestException("Confirm email failure");
            }

            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{user.Id}{GetDeviceInfo()}";
            string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{user.Id}{GetDeviceInfo()}";
            var accessToken = _jwtTokenService.GenerateAccessToken(user, null);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id);
            
            UserDto userResponse = UserExtension.ApplicationUserToUserDto(user);
            return new LoginResponseDto(UserData: userResponse,
                TokenResponse: new TokenResponse(accessToken, refreshToken));
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }

    }

    public async Task<LoginResponseDto> VerifyCodeAsync(VerifyCodeRequestDto verifyCodeRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var verifyCodeCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.VerifyCode}{verifyCodeRequestDto.Email}";
            var cacheResponse = await _cacheService.GetCacheAsync(verifyCodeCacheKey);
            if (string.IsNullOrEmpty(cacheResponse))
            {
                throw new BadRequestException("Invalid code");
            }
            var user = await _userManager.FindByEmailAsync(verifyCodeRequestDto.Email) ?? throw new UserNotFoundException($"{verifyCodeRequestDto.Email}");
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> TwoFactorEnabledOrDisableAsync(CancellationToken cancellationToken = default)
    {
        string userId = _jwtTokenService.GetUserFromClaimToken().Id;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedException();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException("User don't exists");
        }

        user.TwoFactorEnabled = !user.TwoFactorEnabled;
        var response = await _userManager.UpdateAsync(user);
        return response.Succeeded;
    }

    public async Task<LoginResponseDto> VerifyLoginCodeAsync(VerifyLoginCodeRequestDto verifyLoginCodeRequestDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(verifyLoginCodeRequestDto.Email) ??
                       throw new UserNotFoundException(
                           $"Cannot be to find user with email : {verifyLoginCodeRequestDto.Email}");
            string verifyLoginCodeCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.VerifyLoginCode}{user.Id}";
            var code = await _cacheService.GetCacheAsync(verifyLoginCodeCacheKey);
            if (!string.Equals(code, verifyLoginCodeRequestDto.Code))
            {
                throw new BadRequestException("Invalid Code");
            }
            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{user.Id}{GetDeviceInfo()}";
            string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{user.Id}{GetDeviceInfo()}";
            string accessTokenCacheResponse =
                await _cacheService.GetCacheAsync(accessTokenCacheKey);
            string refreshTokenCacheResponse = await _cacheService.GetCacheAsync(refreshTokenCacheKey);
            if (string.IsNullOrEmpty(accessTokenCacheResponse))
            {
                var roles = await _userManager.GetRolesAsync(user);
                accessTokenCacheResponse = _jwtTokenService.GenerateAccessToken(user, roles);
                await _cacheService.SetStringAsync(accessTokenCacheKey, accessTokenCacheResponse, TimeSpan.FromDays(1));
            }
            
            if (string.IsNullOrEmpty(refreshTokenCacheResponse))
            {
                refreshTokenCacheResponse = _jwtTokenService.GenerateRefreshToken(user.Id);
                await _cacheService.SetStringAsync(refreshTokenCacheKey, refreshTokenCacheResponse, TimeSpan.FromMinutes(7));
            }

            var loginResponse = new LoginResponseDto(
                UserData: new UserDto(user.Id, user.UserName!, user.FullName, user.AvatarUrl ?? "", user.BirthDay),
                TokenResponse: new TokenResponse(accessTokenCacheResponse, refreshTokenCacheResponse)
            );


            return loginResponse;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> ReConfirmEmailAsync(ReConfirmEmailRequestDto reConfirmEmailRequestDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(reConfirmEmailRequestDto.Email);
        if (user is null)
        {
            return false;
        }
        var cacheTokenConfirmEmailKey = $"{CacheKey.Domain}{CacheKey.Auth.ConfirmEmail}{reConfirmEmailRequestDto.Email}";
        var tokenConfirmEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _cacheService.SetCacheAsync(cacheTokenConfirmEmailKey, tokenConfirmEmail, TimeSpan.FromMinutes(2.5));

        await _producer.ProduceAsync("verify-account", "token",
            $"https://localhost:7002/api/v1/auth/confirm-email?Token={tokenConfirmEmail}&Email={reConfirmEmailRequestDto.Email}");

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!string.Equals(resetPasswordRequestDto.NewPassword, resetPasswordRequestDto.ConfirmPassword))
            {
                throw new BadRequestException("Password dont not matching");
            }
            var userFound = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (userFound is null)
            {
                throw new UserNotFoundException($"Could not find user with email is {resetPasswordRequestDto.Email}");
            }
            var cacheToken = $"{CacheKey.Domain}{CacheKey.Auth.ForgotPassword}{userFound.Email}";
            var cacheTokenResult = (await _cacheService.GetCacheAsync(cacheToken));
            var token = cacheTokenResult.Substring(1, cacheTokenResult.Length - 2);
            if (string.IsNullOrEmpty(token) || !string.Equals(token, resetPasswordRequestDto.Token))
            {
                throw new BadRequestException("Invalid token");
            }

            var changePass =
                await _userManager.ResetPasswordAsync(userFound, token, resetPasswordRequestDto.NewPassword);

            return changePass.Succeeded;


        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<UserDto>> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _userManager.Users.Where(u => u.Email!.Contains(email)).ToListAsync(cancellationToken);
            var result = users.Where(r => r.Email!.Contains(email) && ((email.Length / r.Email.Length) * 100 > 90));
            var response = _mapper.Map<IEnumerable<UserDto>>(result);
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<DeviceLoginResponseDto>> GetDeviceLoginByUserAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var userFound = await _userManager.FindByEmailAsync(email);
            if (userFound is null)
            {
                throw new UserNotFoundException("Invalid user");
            }
            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{userFound.Id}";
            var tokens = await _cacheService.GetKeyStringsAsync(accessTokenCacheKey);
            IList<DeviceLoginResponseDto> responseDtos = [];
            foreach (var token in tokens)
            {
                responseDtos.Add(new DeviceLoginResponseDto(token, token));
            }

            return responseDtos.AsEnumerable();

        }
        catch (Exception e)
        {
            throw new BadRequestException($"{e.Message} {nameof(GetDeviceLoginByUserAsync)}");
        }
    }

    public async Task<bool> LogoutDeviceAsync(IEnumerable<DeviceLoginResponseDto> deviceLoginResponseDtos, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var deviceLoginResponseDto in deviceLoginResponseDtos)
            {
                await _cacheService.RemoveCacheAsync(deviceLoginResponseDto.Key);
            }
            return true;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> SwitchTwoFactorAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = _jwtTokenService.GetUserFromClaimToken().Id;
            ArgumentException.ThrowIfNullOrEmpty(userId);
            var userFound = await _userManager.FindByIdAsync(userId);
            if (userFound is null)
            {
                throw new UserNotFoundException($"Cloud not be to find user with id {userId}");
            }

            userFound.TwoFactorEnabled = !userFound.TwoFactorEnabled;
            IdentityResult isUpdated = await _userManager.UpdateAsync(userFound);
            return isUpdated.Succeeded;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<TokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var headers = _contextAccessor.HttpContext!.Request.Headers;

            if (headers.TryGetValue(RequestHeaderName.X_CLIENT_REQUEST, out var clientRequest) &&
                headers.TryGetValue(RequestHeaderName.X_REFRESH_TOKEN, out var refreshToken))
            {
                var userIdFromHeader = clientRequest.ToString();
                var refreshTokenValue = refreshToken.ToString();

                string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{userIdFromHeader}{GetDeviceInfo()}";
                string refreshTokenCacheResponse = await _cacheService.GetCacheAsync(refreshTokenCacheKey);
                string refreshTokenC = refreshTokenCacheResponse.Substring(1, refreshTokenCacheResponse.Length - 2);
                if (!string.Equals(refreshTokenValue, refreshTokenC))
                {
                    throw new BadRequestException("Invalid token");
                }

                (string, string) tokenResult = await StoreToken(userIdFromHeader);
                return new TokenResponse(tokenResult.Item1, tokenResult.Item2);
            }
            else
            {
                throw new Exception("Missing required headers");
            }
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> ChangePasswordForAdminAsync(ChangePasswordForAdminRequestDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                throw new UserNotFoundException("Cannot be to find user with card id");
            }

            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                throw new BadRequestException("Password don't matching");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var isUpdatePassword = await _userManager.ResetPasswordAsync(user, token, request.Password);
            return isUpdatePassword.Succeeded;

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<UserDto> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var userFound = await _userManager.Users.Select(r => new
            {
                Id = r.Id,
                FullName = r.FullName,
                Avatar = r.AvatarUrl,
                BirthDay = r.BirthDay
            }).Where(r => r.Id == id).SingleOrDefaultAsync(cancellationToken);
            
            if (userFound is null)
            {
                throw new UserNotFoundException("User not found");
            }
            
            return new UserDto(userFound.Id,"",  userFound.FullName, userFound.Avatar, userFound.BirthDay);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<UserShareResponseDto>> GetUserShareAsync(UserShareRequestDto query, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.Users.Select(r => new
            {
                Id = r.Id,
                FullName = r.FullName,
                Avatar = r.AvatarUrl
            }).Where(r => query.Ids.Contains(r.Id)).ToListAsync(cancellationToken);
            var response = user.Select(r => new UserShareResponseDto(r.Id, r.FullName, r.Avatar)).ToList();
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }


    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginRequestDto.UserName || u.Email == loginRequestDto.UserName, cancellationToken: cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException("User cannot register");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isPasswordValid)
            {
                user.LockoutEnabled = true;
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= 5)
                {
                    user.LockoutEnd = DateTimeOffset.Now.AddMinutes(5);
                }
                await _userManager.UpdateAsync(user);
                throw new BadRequestException("Password is incorrect");
            }
            if (!user.EmailConfirmed)
            {
                throw new BadRequestException($"Account with email {user.Email} is not confirm");
            }
            if (user.LockoutEnd > DateTimeOffset.Now)
            {
                throw new BadRequestException($"Account with email {user.Email} is not locked");
            }

            if (user.TwoFactorEnabled)
            {
                string verifyLoginCodeCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.VerifyLoginCode}{user.Id}";
                const string chars = "0123456789";
                Random random = new Random();
                string code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                await _cacheService.SetStringAsync(verifyLoginCodeCacheKey, code, TimeSpan.FromMinutes(1.5));
                // publish topic in kafka
                MailRequestDto verifyCodeLoginMailRequest = new MailRequestDto(user.Email!, "Mã login", code);
                string mailSerialization = JsonConvert.SerializeObject(verifyCodeLoginMailRequest);
                await _producer.ProduceAsync("send-mail", "verify-code-login", mailSerialization);
                return new LoginResponseDto(null, null);
            }

            switch (user.Status)
            {
                
            }
            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{user.Id}{GetDeviceInfo()}";
            Log.Information("KEY: " + accessTokenCacheKey);
            string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{user.Id}{GetDeviceInfo()}";
            string accessTokenCacheResponse =
                await _cacheService.GetCacheAsync(accessTokenCacheKey);
            string refreshTokenCacheResponse = await _cacheService.GetCacheAsync(refreshTokenCacheKey);
            if (string.IsNullOrEmpty(accessTokenCacheResponse))
            {
                var roles = await _userManager.GetRolesAsync(user);
                accessTokenCacheResponse = _jwtTokenService.GenerateAccessToken(user, roles);
                await _cacheService.SetStringAsync(accessTokenCacheKey, accessTokenCacheResponse, TimeSpan.FromDays(1));
            }
            
            if (string.IsNullOrEmpty(refreshTokenCacheResponse))
            {
                refreshTokenCacheResponse = _jwtTokenService.GenerateRefreshToken(user.Id);
                await _cacheService.SetStringAsync(refreshTokenCacheKey, refreshTokenCacheResponse, TimeSpan.FromMinutes(7));
            }

            var loginResponse = new LoginResponseDto(
                UserData: new UserDto(user.Id, user.UserName!, user.FullName, user.AvatarUrl ?? "", user.BirthDay),
                TokenResponse: new TokenResponse(accessTokenCacheResponse, refreshTokenCacheResponse)
            );
            
            

            return loginResponse;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
    

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default)
    {
        if(DateTime.TryParse(registerRequestDto.BirthDay, out DateTime bd))
        {
            if (bd > DateTime.UtcNow)
            {
                throw new BadRequestException("BirthDay is invalid");
            }
            else
            {
                throw new BadRequestException("BirthDay is invalid 1");
            }
        }
        var userExists = await _userManager.FindByEmailAsync(registerRequestDto.Email);
       
            
        if(userExists is not null)
        {
            throw new BadRequestException($"Account with email {registerRequestDto.Email} is exits");
        }

        var user = new ApplicationUser
        {
            Email = registerRequestDto.Email,
            UserName = registerRequestDto.UserName,
            FullName = registerRequestDto.FullName,
            AvatarUrl = registerRequestDto.AvatarUrl ?? "",
            BirthDay = registerRequestDto.BirthDay,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (!result.Succeeded) {
                throw new BadRequestException(result.Errors.FirstOrDefault()!.Description.ToString());
            }

            var cacheTokenConfirmEmailKey = $"{CacheKey.Domain}{CacheKey.Auth.ConfirmEmail}{user.Email}";
            var tokenConfirmEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _cacheService.SetCacheAsync(cacheTokenConfirmEmailKey, tokenConfirmEmail, TimeSpan.FromMinutes(2.5));

            await _producer.ProduceAsync("verify-account", "token",
                $"https://localhost:7002/api/v1/auth/confirm-email?Token={tokenConfirmEmail}&Email={registerRequestDto.Email}");

            var registerResponse = new RegisterResponseDto(
                UserData: new UserDto(user.Id, user.UserName, user.FullName, user.AvatarUrl, user.BirthDay), Token: new TokenResponse("", ""));

            return registerResponse;
        }
        catch (Exception ex) 
        {
            throw new BadRequestException(ex.Message.ToString());
        }
    }


    private async Task<(string, string)> StoreToken(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{user.Id}{GetDeviceInfo()}";
        string refreshTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.RefreshToken}{user.Id}{GetDeviceInfo()}";
        string accessTokenCacheResponse =
            await _cacheService.GetCacheAsync(accessTokenCacheKey);
        string refreshTokenCacheResponse = await _cacheService.GetCacheAsync(refreshTokenCacheKey);
        if (string.IsNullOrEmpty(accessTokenCacheResponse))
        {
            var roles = await _userManager.GetRolesAsync(user);
            accessTokenCacheResponse = _jwtTokenService.GenerateAccessToken(user, roles);
            await _cacheService.SetStringAsync(accessTokenCacheKey, accessTokenCacheResponse, TimeSpan.FromDays(1));
        }
            
        if (string.IsNullOrEmpty(refreshTokenCacheResponse))
        {
            refreshTokenCacheResponse = _jwtTokenService.GenerateRefreshToken(user.Id);
            await _cacheService.SetStringAsync(refreshTokenCacheKey, refreshTokenCacheResponse, TimeSpan.FromMinutes(7));
        }

        return (accessTokenCacheResponse, refreshTokenCacheResponse);
    } 
}
