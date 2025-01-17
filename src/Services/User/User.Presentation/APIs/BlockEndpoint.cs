using User.Presentation.Abstractions;

namespace User.Presentation.APIs;

public class BlockEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi(EndpointNameBase.Block.Name)
            .MapGroup(EndpointNameBase.Block.BaseUrl).HasApiVersion(1);
    }
}