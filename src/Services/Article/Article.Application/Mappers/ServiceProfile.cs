using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Domain.Models;
using AutoMapper;

namespace Article.Application.Mappers;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        #region Category

        CreateMap<CreateCategoryRequestDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<Category, CategoryResponseBaseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value.ToString()));
        #endregion
    }
}