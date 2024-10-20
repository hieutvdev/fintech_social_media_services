using Auth.Application.DTOs.Request.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.ReConfirmEmail;

public record ReConfirmEmailCommand(ReConfirmEmailRequestDto ReConfirmEmailRequestDto) : ICommand<bool>;