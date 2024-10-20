using Auth.Application.UseCases.V1.Commands.Auth.ForgotPassword;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(r => r.ForgotPasswordRequestDto)
            .NotEmpty()
            .WithMessage("Data payload cannot be null");

        RuleFor(r => r.ForgotPasswordRequestDto.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty or whitespace");
    }
}