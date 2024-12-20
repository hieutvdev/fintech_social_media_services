using Article.Application.DTOs.Request.Tag;
using Article.Application.DTOs.Response.Tag;

namespace Article.Application.Mappers.TagMapper;

public static class TagMapperDto
{
    public static void UpdateCategoryWithNewValues(Domain.Models.Tag tag,
        UpdateTagRequestDto updateCategoryRequestDto)
    {
        tag.Update(updateCategoryRequestDto.Name);
    }

    public static TagResponseBaseDto TagToBaseDto(this Tag tag)
    {
        return new TagResponseBaseDto(tag.Id.Value.ToString(), tag.Name, tag.Slug);
    }
}