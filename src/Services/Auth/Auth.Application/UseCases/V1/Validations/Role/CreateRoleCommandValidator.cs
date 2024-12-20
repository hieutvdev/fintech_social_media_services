using Auth.Application.UseCases.V1.Commands.Role.CreateRole;
using FluentValidation;

namespace Auth.Application.UseCases.V1.Validations.Role;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(r => r.CreateRoleRequestDto)
            .NotEmpty()
            .WithMessage("Data create is empty or whitespace");

        RuleFor(r => r.CreateRoleRequestDto.RoleName)
            .NotEmpty()
            .WithMessage("Role name is not null");
        
    }
}