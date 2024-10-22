using Article.Application.DTOs.Request.Category;

namespace Article.Application.Mappers.CategoryMapper;

public static class CategoryMapperDto
{
    public static void UpdateCategoryWithNewValues(Domain.Models.Category category,
        UpdateCategoryRequestDto updateCategoryRequestDto)
    {
        category.Update(updateCategoryRequestDto.Name, updateCategoryRequestDto.Description, updateCategoryRequestDto.CategoryStatus);
    }
    
}