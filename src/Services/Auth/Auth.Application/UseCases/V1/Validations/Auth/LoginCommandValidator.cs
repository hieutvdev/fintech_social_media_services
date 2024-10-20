using Auth.Application.UseCases.V1.Commands.Auth.Login;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginRequestDto.UserName)
            .NotEmpty()
            .WithMessage("Username or email cannot be empty");

        RuleFor(x => x.LoginRequestDto.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}