using Auth.Application.UseCases.V1.Commands.Auth.RegisterUser;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Auth;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.User.UserName).NotEmpty().WithMessage("Username cannot empty");
        RuleFor(x => x.User.Password).NotEmpty().WithMessage("Password cannot empty");
        RuleFor(x => x.User.BirthDay).NotEmpty().WithMessage("BirthDay cannot empty");
        RuleFor(x => x.User.FullName).NotEmpty().WithMessage("Fullname cannot empty");
    }
}