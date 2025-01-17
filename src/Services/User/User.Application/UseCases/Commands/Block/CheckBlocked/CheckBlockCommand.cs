namespace User.Application.UseCases.Commands.Block.CheckBlocked;

public record CheckBlockCommand(string UserId) : ICommand<bool>;