using Article.Application.DTOs.Request.Category;
using BuildingBlocks.Helpers;
using FluentValidation;

namespace Article.Application.UseCases.V1.Validations.Categories;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryRequestDto>
{
    public UpdateCategoryCommandValidator()
    {
        
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageFieldEmpty(nameof(UpdateCategoryRequestDto.Id)));
        
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageFieldEmpty(nameof(UpdateCategoryRequestDto.Name)));
    }
}