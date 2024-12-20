using BuildingBlocks.CQRS.Commands;
using Mail.API.Helpers;

namespace Mail.API.UseCases.Commands.Mail;

public record SendMailCommand(MailRequest MailRequest) : ICommand<bool>;
