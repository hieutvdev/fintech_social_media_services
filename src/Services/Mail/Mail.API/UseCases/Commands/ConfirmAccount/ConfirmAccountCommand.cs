using BuildingBlocks.CQRS.Commands;

namespace Mail.API.UseCases.Commands.ConfirmAccount;

public record ConfirmAccountCommand(string Url) : ICommand;