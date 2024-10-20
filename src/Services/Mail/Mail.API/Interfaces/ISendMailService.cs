using Mail.API.Helpers;

namespace Mail.API.Interfaces;

public interface ISendMailService
{
    Task SendMailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default!);
    Task SendMailConfirmAccountAsync(string url, CancellationToken cancellationToken = default!);

    Task StartConsumingAsync(CancellationToken cancellationToken = default!);
}