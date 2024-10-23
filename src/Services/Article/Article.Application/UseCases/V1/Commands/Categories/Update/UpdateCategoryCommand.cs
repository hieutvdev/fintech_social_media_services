using Article.Application.DTOs.Request.Category;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Categories.Update;

public record UpdateCategoryCommand(UpdateCategoryRequestDto UpdateCategoryRequestDto) : ICommand<bool>;