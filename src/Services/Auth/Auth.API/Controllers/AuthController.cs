
using Auth.API.Attributes;
using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;
using Auth.Application.UseCases.V1.Commands.Auth.ChangePassword;
using Auth.Application.UseCases.V1.Commands.Auth.ConfirmEmail;
using Auth.Application.UseCases.V1.Commands.Auth.ForgotPassword;
using Auth.Application.UseCases.V1.Commands.Auth.Login;
using Auth.Application.UseCases.V1.Commands.Auth.LogoutDevice;
using Auth.Application.UseCases.V1.Commands.Auth.ReConfirmEmail;
using Auth.Application.UseCases.V1.Commands.Auth.RegisterUser;
using Auth.Application.UseCases.V1.Commands.Auth.ResetPassword;
using Auth.Application.UseCases.V1.Commands.Auth.SwitchTwoFactor;
using Auth.Application.UseCases.V1.Commands.Auth.VerifyCodeLogin;
using Auth.Application.UseCases.V1.Queries.Auth.GetDeviceByUserLogin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : Controller
{
    
    [HttpGet("get-device-user/{email}")]
    public async Task<IActionResult> GetDevicesUserLogin(string email)
    {
        var result = await mediator.Send(new GetDeviceByUserLoginQuery(Email: email));
        return Ok(result);
    }
    
    [HttpPost("logout-device")]
    public async Task<IActionResult> LogoutDevice(IEnumerable<DeviceLoginResponseDto> deviceLoginResponseDtos)
    {
        var result = await mediator.Send(new LogoutDeviceCommand(deviceLoginResponseDtos));
        return Ok(result);
    }
    
    [HttpPatch("switch-two-factor")]
    public async Task<IActionResult> SwitchTwoFactor()
    {
        var result = await mediator.Send(new SwitchTwoFactorCommand());
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("test-auth")]
    public IActionResult TestAuth()
    {
        
        return Ok("ok");
    }
    
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
    {
        var result = await mediator.Send(new RegisterUserCommand(User: registerRequestDto));
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        var result = await mediator.Send(new LoginCommand(LoginRequestDto: loginRequestDto));
        
        if (result.IsFailure)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePassword)
    {
        var result = await mediator.Send(new ChangePasswordCommand(ChangePasswordRequestDto: changePassword));

        return Ok(result);
    }  
    
    
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequestDto confirmEmailRequest)
    {
        var result = await mediator.Send(new ConfirmEmailCommand(ConfirmEmailRequestDto: confirmEmailRequest));

        return Ok(result);
    }  
    
    [HttpPost("verify-code-login")]
    public async Task<IActionResult> VerifyCodeLogin(VerifyLoginCodeRequestDto verifyLoginCodeRequestDto)
    {
        var result = await mediator.Send(new VerifyCodeLoginCommand(verifyLoginCodeRequestDto: verifyLoginCodeRequestDto));

        return Ok(result);
    }  
    
    [HttpPost("re-confirm-email")]
    public async Task<IActionResult> ReConfirmEmail(ReConfirmEmailRequestDto reConfirmEmailRequestDto)
    {
        var result = await mediator.Send(new ReConfirmEmailCommand(ReConfirmEmailRequestDto: reConfirmEmailRequestDto));

        return Ok(result);
    }  
    
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto forgotPasswordRequest)
    {
        var result = await mediator.Send(new ForgotPasswordCommand(ForgotPasswordRequestDto: forgotPasswordRequest));

        return Ok(result);
    }  
    
    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
    {
        var result = await mediator.Send(new ResetPasswordCommand(ResetPasswordRequestDto: resetPasswordRequestDto));

        return Ok(result);
    }  
}