using FluentValidation;
using User.Application.DTOs.Request.Follow;
using User.Application.UseCases.Commands.Follow.Create;

namespace User.Application.UseCases.Validators.Follow;

public class CreateFollowCommandValidator : AbstractValidator<CreateFollowCommand>
{
    public CreateFollowCommandValidator()
    {
        
    }
}