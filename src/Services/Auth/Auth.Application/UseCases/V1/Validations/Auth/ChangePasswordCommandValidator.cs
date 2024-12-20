
using Auth.Application.UseCases.V1.Commands.Auth.ChangePassword;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.ChangePasswordRequestDto.OldPassword)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
        RuleFor(x => x.ChangePasswordRequestDto.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword cannot be empty");
        RuleFor(x => x.ChangePasswordRequestDto.ConfirmPassword)
            .NotEmpty()
            .WithMessage("ConfirmPassword cannot be empty");
    }
}
