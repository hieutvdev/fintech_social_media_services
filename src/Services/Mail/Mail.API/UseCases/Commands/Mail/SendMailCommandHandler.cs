using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Mail.API.Interfaces;

namespace Mail.API.UseCases.Commands.Mail;

public class SendMailCommandHandler(ISendMailService sendMailService)
: ICommandHandler<SendMailCommand, bool>
{
    public async Task<ResultT<bool>> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
        await sendMailService.SendMailAsync(request.MailRequest, cancellationToken);
        var response = Result.Create(true);
        return response;
    }
}