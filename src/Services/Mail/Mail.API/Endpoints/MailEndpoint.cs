using Carter;
using Mail.API.Helpers;
using Mail.API.UseCases.Commands.Mail;
using MediatR;

namespace Mail.API.Endpoints;

public class MailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/mail", async (MailRequest mailRequest, ISender sender) =>
        {
            var response = await sender.Send(new SendMailCommand(mailRequest));
            return Results.Ok(response);
        }).WithName("SendMail");
    }
}