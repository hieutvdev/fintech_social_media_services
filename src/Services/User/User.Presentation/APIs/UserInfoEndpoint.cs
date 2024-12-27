using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace User.Presentation.APIs;

public class UserInfoEndpoint : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/user-info";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("user-info")
            .MapGroup(BaseUrl).HasApiVersion(1);


        group.MapPost("/ping", async () =>
        {

            return Results.Ok("Pogn");
        }).WithName("CreateArticle");

    }
}