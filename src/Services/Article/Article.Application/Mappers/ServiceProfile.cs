using Article.Application.DTOs.Request.Article;
using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Request.Tag;
using Article.Application.DTOs.Response.Category;
using Article.Application.DTOs.Response.Tag;
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
            .ForMember(dest => dest.Slug, opt => opt.Ignore())  // Slug is generated in the entity
            .ForMember(dest => dest.Id, opt => opt.Ignore())     // Id is assigned elsewhere
            .ForMember(dest => dest.CategoryStatus, opt => opt.Ignore());
        
        CreateMap<Category, CategoryResponseBaseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value.ToString())) 
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.CategoryStatus))
            .ForMember(dest => dest.LastUpdateAt, opt => opt.Ignore());
        #endregion

        #region Tag

        CreateMap<CreateTagRequestDto, Tag>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.Ignore()) // Slug is generated in the entity
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id is assigned elsewhere

        CreateMap<Tag, TagResponseBaseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug));

        #endregion

        #region Article

        CreateMap<CreateArticleRequestDto, Domain.Models.Article>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
            .ForMember(dest => dest.Slug, opt => opt.Ignore()) // Slug is generated in the entity
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id is assigned elsewhere

        CreateMap<Tag, TagResponseBaseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug));

        #endregion
    }
}