using Article.Domain.Enums;

namespace Article.Application.DTOs.Request.Category;

public record UpdateCategoryRequestDto(string Id, string Name, string Description, CategoryStatus CategoryStatus);