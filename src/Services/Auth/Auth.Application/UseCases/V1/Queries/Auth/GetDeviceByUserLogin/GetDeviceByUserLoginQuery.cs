using System.Collections;
using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Queries.Auth.GetDeviceByUserLogin;

public record GetDeviceByUserLoginQuery(string Email) : IQuery<IEnumerable<DeviceLoginResponseDto>>; 