using Carter;
using Mail.API.Helpers;
using Mail.API.UseCases.Commands.ConfirmAccount;
using MediatR;

namespace Mail.API.Endpoints;

public record ConfirmAccountRequest(string Url);

public class AuthEndpoint : ICarterModule
{
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/email/confirm-account", async (ConfirmAccountRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ConfirmAccountCommand(request.Url));
            return Results.Ok(result);
        })
            .WithName("ConfirmEmail");
    }
}