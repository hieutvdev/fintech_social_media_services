using Article.Application.DTOs.Request.Category;
using Article.Application.UseCases.V1.Commands.Categories.Create;
using BuildingBlocks.Helpers;
using FluentValidation;

namespace Article.Application.UseCases.V1.Validations.Categories;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.CreateCategoryRequestDto)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageObjectEmpty());
        RuleFor(c => c.CreateCategoryRequestDto.Name)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageFieldEmpty(nameof(CreateCategoryRequestDto.Name)));
        
        RuleFor(c => c.CreateCategoryRequestDto.Description)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageFieldEmpty(nameof(CreateCategoryRequestDto.Description)));
    }
}