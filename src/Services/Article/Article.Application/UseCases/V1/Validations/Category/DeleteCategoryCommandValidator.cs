using Article.Application.UseCases.V1.Commands.Categories.Delete;
using FluentValidation;

namespace Article.Application.UseCases.V1.Validations.Category;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(r => r.Ids)
            .Must(r => r.Any())
            .WithMessage("Data delete cannot be null");
    }
}