using Auth.Application.DTOs.Request.Role;
using Auth.Application.UseCases.V1.Commands.Role.AssignRole;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Role;

public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(r => r.AssignRoleRequestDto)
            .NotEmpty()
            .WithMessage("Data payload is null or empty");

        RuleFor(r => r.AssignRoleRequestDto.UserName)
            .NotEmpty()
            .WithMessage("Username cannot be empty or whitespace");

        RuleFor(r => r.AssignRoleRequestDto.RoleNames)
            .NotEmpty()
            .WithMessage("Rolenames cannot be null")
            .Must(r => r.Any())
            .WithMessage("Role name must not be empty");
    }
}