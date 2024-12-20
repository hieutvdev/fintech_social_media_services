using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;
using UserDto = Auth.Application.DTOs.Response.Auth.UserDto;

namespace Auth.Application.Services;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default!);
    
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default!);

    Task<bool> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto,
        CancellationToken cancellationToken = default!);

    Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto,
        CancellationToken cancellationToken = default!);

    Task<LoginResponseDto> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto,
        CancellationToken cancellationToken = default!);

    Task<LoginResponseDto> VerifyCodeAsync(VerifyCodeRequestDto verifyCodeRequestDto,
        CancellationToken cancellationToken = default!);

    Task<bool> TwoFactorEnabledOrDisableAsync(CancellationToken cancellationToken = default!);

    Task<LoginResponseDto> VerifyLoginCodeAsync(VerifyLoginCodeRequestDto verifyLoginCodeRequestDto,
        CancellationToken cancellationToken = default!);

    Task<bool> ReConfirmEmailAsync(ReConfirmEmailRequestDto reConfirmEmailRequestDto,  CancellationToken cancellationToken = default!);

    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto,
        CancellationToken cancellationToken = default!);

    Task<IEnumerable<UserDto>> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default!);

    Task<IEnumerable<DeviceLoginResponseDto>> GetDeviceLoginByUserAsync(string email,
        CancellationToken cancellationToken = default!);

    Task<bool> LogoutDeviceAsync(IEnumerable<DeviceLoginResponseDto> deviceLoginResponseDtos,
        CancellationToken cancellationToken = default!);

    Task<bool> SwitchTwoFactorAsync(CancellationToken cancellationToken = default!);

    Task<TokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default!);

    Task<bool> ChangePasswordForAdminAsync(ChangePasswordForAdminRequestDto request,
        CancellationToken cancellationToken = default!
    );

}