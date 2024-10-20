using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.LogoutDevice;

public record LogoutDeviceCommand(IEnumerable<DeviceLoginResponseDto> DeviceLoginResponseDtos) : ICommand;