using Auth.Application.UseCases.V1.Commands.Auth.ReConfirmEmail;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class ReConfirmEmailCommandValidator : AbstractValidator<ReConfirmEmailCommand>
{
    public ReConfirmEmailCommandValidator()
    {
        RuleFor(r => r.ReConfirmEmailRequestDto)
            .NotEmpty()
            .WithMessage("Data payload is null");

        RuleFor(r => r.ReConfirmEmailRequestDto.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty or whitespace");
    }
}