using BuildingBlocks.DependencyInjection.Extensions;
using BuildingBlocks.Logging;
using Serilog;
using User.Application.DependencyInjection.Extensions;
using User.Infrastructure.DependencyInjection.Extensions;
using User.Persistence.DependencyInjection.Extensions;
using User.Presentation.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSerilogService(builder.Configuration, builder.Host);



builder.Services
    .AddApplicationService(builder.Configuration)
    .AddPersistenceService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddPresentationService(builder.Configuration)
    .AddSwaggerOptions();


builder.Services.AddAuthorization();

var app = builder.Build();

app.UsePresentationService();
app.UseApplicationService();
app.UseInfrastructureService();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


try
{
    Log.Information("Application Starting");
    await app.RunAsync();
    Log.Information("Application Started");
}catch(Exception e)
{
    Log.Fatal(e, "Application start Failed");
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
public partial class Program { }