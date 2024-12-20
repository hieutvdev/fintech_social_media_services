using Auth.Application.DTOs.Response.Auth;
using Auth.Application.UseCases.V1.Commands.Auth.LogoutDevice;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class LogoutDeviceCommandValidator : AbstractValidator<LogoutDeviceCommand>
{
    public LogoutDeviceCommandValidator()
    {
        RuleFor(r => r.DeviceLoginResponseDtos)
            .NotEmpty()
            .WithMessage("Device list must not be empty")
            .Must(x => x.Any()).WithMessage("Device list cannot be empty");
        
    }
}

public class DeviceLoginResponseDtoValidator : AbstractValidator<DeviceLoginResponseDto>
{
    public DeviceLoginResponseDtoValidator()
    {
        RuleFor(r => r.Key)
            .NotEmpty()
            .WithMessage("key must not be empty");

        RuleFor(r => r.DeviceName)
            .NotEmpty()
            .WithMessage("DeviceName must not be empty");
    }
}