using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Domain.Models;

namespace Article.Application.Mappers.CategoryMapper;

public static class CategoryMapperDto
{
    public static void UpdateCategoryWithNewValues(Domain.Models.Category category,
        UpdateCategoryRequestDto updateCategoryRequestDto)
    {
        category.Update(updateCategoryRequestDto.Name, updateCategoryRequestDto.Description, updateCategoryRequestDto.CategoryStatus);
    }

    public static CategoryResponseBaseDto CategoryToBaseDto(this Category category)
    {
        return new CategoryResponseBaseDto(category.Id.Value.ToString(), category.Name, category.Slug, category.Description, (int)category.CategoryStatus, category.UpdatedAt);
    }
    
}

