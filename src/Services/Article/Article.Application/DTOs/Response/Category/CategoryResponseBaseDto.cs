using Article.Domain.Enums;

namespace Article.Application.DTOs.Response.Category;

public record CategoryResponseBaseDto(
    string Id,
    string Name,
    string Slug,
    string Description,
    CategoryStatus Status,
    DateTime? LastUpdateAt);
