using BuildingBlocks.Messaging.Messaging.Kafka;
using Mail.API.Configurations;
using Mail.API.Helpers;
using Mail.API.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Mail.API.Services;

public class SendMailService : ISendMailService
{
    private readonly IKafkaConsumerService<string, string> _consumer;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<SendMailService> _logger;

    public SendMailService(IOptions<EmailSettings> emailSettings, IKafkaConsumerService<string, string> consumer, ILogger<SendMailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _consumer = consumer;
        _logger = logger;
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
    {
        
        _consumer.Subscribe("verify-account");
        
        await _consumer.ConsumeAsync(async (key, value) =>
        {
            await SendMailConfirmAccountAsync(value, cancellationToken); 
        });
    }

    public async Task SendMailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();

        email.Sender = MailboxAddress.Parse(_emailSettings.Email);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password, cancellationToken);

        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendMailConfirmAccountAsync(string url, CancellationToken cancellationToken = default)
    {
        var email = "nguyenhoang.miyuka@gmail.com";
        var mailRequest = new MailRequest
        {
            ToEmail = email,
            Body = "Hoang bu lon",
            Subject = "Bu lon di"
        };
        await SendMailAsync(mailRequest, cancellationToken);
        _logger.LogInformation("Send successful----------------------");
    }
    
   
}
