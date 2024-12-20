using Auth.Application.UseCases.V1.Commands.Auth.ConfirmEmail;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(r => r.ConfirmEmailRequestDto)
            .NotEmpty()
            .WithMessage("Data confirm email is null");

        RuleFor(r => r.ConfirmEmailRequestDto.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty or whitespace");

        RuleFor(r => r.ConfirmEmailRequestDto.Token)
            .NotEmpty()
            .WithMessage("Token cannot be empty or whitespace");
    }
}