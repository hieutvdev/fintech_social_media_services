namespace Auth.Application.DTOs.Request.MailService;

public record MailRequestDto(string ToEmail, string Subject, string Body);