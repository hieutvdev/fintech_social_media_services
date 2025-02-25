﻿using BuildingBlocks.Messaging.MessageModels.AuthService;
using BuildingBlocks.Messaging.Messaging.Kafka;
using Mail.API.Configurations;
using Mail.API.Helpers;
using Mail.API.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;

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
        try
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
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    public async Task SendMailConfirmAccountAsync(string url, CancellationToken cancellationToken = default)
    {
        // var email = "nguyenhoang.miyuka@gmail.com";
        var email = "trinhhieu758@gmail.com";
        var auth = JsonConvert.DeserializeObject<AuthRegisterRequestDto>(url);
        var mailRequest = new MailRequest
        {
            ToEmail = email,
            Body = "Hoang bu lon",
            Subject = "Hiiiiiiiii "
        };
        await SendMailAsync(mailRequest, cancellationToken);
        _logger.LogInformation("Send successful----------------------");
    }
    
   
}
