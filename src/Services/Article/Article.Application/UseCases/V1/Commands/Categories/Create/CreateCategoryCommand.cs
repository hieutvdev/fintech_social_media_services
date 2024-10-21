
using Article.Application.DTOs.Request.Category;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Categories.Create;

public record CreateCategoryCommand(CreateCategoryRequestDto CreateCategoryRequestDto) : ICommand;