using Auth.Application.DTOs.Response.Auth;
using Auth.Application.DTOs.Response.Role;
using AutoMapper;

namespace Auth.Application.Mappers;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {


        CreateMap<UserDto, ApplicationUser>().ReverseMap();
        CreateMap<RoleResponseDto, ApplicationRole>().ReverseMap();
    }
}