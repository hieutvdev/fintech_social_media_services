using Auth.Application.UseCases.V1.Commands.Role.DeleteRole;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Role;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(r => r.RoleName)
            .NotEmpty()
            .WithMessage("Role name cannot be null or empty");
    }
}