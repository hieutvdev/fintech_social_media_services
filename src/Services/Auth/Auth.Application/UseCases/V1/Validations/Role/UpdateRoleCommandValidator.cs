using Auth.Application.UseCases.V1.Commands.Role.UpdateRole;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Role;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(r => r.UpdateRoleRequestDto)
            .NotEmpty()
            .WithMessage("Data payload cannot be null or empty");

        RuleFor(r => r.UpdateRoleRequestDto.RoleName)
            .NotEmpty()
            .WithMessage("Role name cannot be null or empty");
    }
}