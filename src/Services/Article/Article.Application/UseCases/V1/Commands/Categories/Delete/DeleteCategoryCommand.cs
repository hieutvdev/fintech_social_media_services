

using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Categories.Delete;

public record DeleteCategoryCommand(string[] Ids) : ICommand<bool>;