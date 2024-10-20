using Auth.Application.UseCases.V1.Commands.Auth.ResetPassword;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(r => r.ResetPasswordRequestDto)
            .NotNull()
            .WithMessage("Data payload is null");
        RuleFor(r => r.ResetPasswordRequestDto.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty or whitespace");
        RuleFor(r => r.ResetPasswordRequestDto.NewPassword)
            .NotEmpty()
            .WithMessage("New password cannot be empty or whitespace");

        RuleFor(r => r.ResetPasswordRequestDto.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password cannot be empty or whitespace");

        RuleFor(r => r.ResetPasswordRequestDto.Token)
            .NotEmpty()
            .WithMessage("Token cannot be empty or whitespace");
    }
}