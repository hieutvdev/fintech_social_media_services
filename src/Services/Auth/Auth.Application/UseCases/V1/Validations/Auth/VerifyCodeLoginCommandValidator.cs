using Auth.Application.UseCases.V1.Commands.Auth.VerifyCodeLogin;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class VerifyCodeLoginCommandValidator : AbstractValidator<VerifyCodeLoginCommand>
{
    public VerifyCodeLoginCommandValidator()
    {
        RuleFor(r => r.verifyLoginCodeRequestDto)
            .NotEmpty()
            .WithMessage("VerifyCodeLoginCommandModel must not be empty");

        RuleFor(r => r.verifyLoginCodeRequestDto.Email)
            .NotEmpty()
            .WithMessage("Email must not be empty");

        RuleFor(r => r.verifyLoginCodeRequestDto.Code)
            .NotEmpty()
            .WithMessage("Code must not be empty");
    }
}