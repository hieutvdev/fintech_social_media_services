using Article.Application.UseCases.V1.Queries.Categories.Detail;
using BuildingBlocks.Helpers;
using FluentValidation;

namespace Article.Application.UseCases.V1.Validations.Category;

public class GetDetailCategoryQueryValidator : AbstractValidator<GetDetailCategoryQuery>
{
    public GetDetailCategoryQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(FluentValidationGenContent.GenMessageFieldEmpty(nameof(GetDetailCategoryQuery.Id)));
    }
}