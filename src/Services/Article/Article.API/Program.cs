using Article.Application.DependencyInjection.Extensions;
using Article.Infrastructure.Data.Extensions;
using Article.Infrastructure.DependencyInjection.Extensions;
using BuildingBlocks.DependencyInjection.Extensions;
using BuildingBlocks.Logging;
using BuildingBlocks.Middleware;
using Carter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSerilogService(builder.Configuration, builder.Host);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5083); 
    
});

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(5083); 
//     options.ListenAnyIP(8002);
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddSwaggerOptions();



builder.Services.AddApiVersioningService();

builder.Services.AddAuthorization();

var app = builder.Build();

MigrationHelper.ApplyMigrations(app);
app.UseForwardedHeaders();
app.UseApplicationService();
app.MapCarter();
app.UseRouting();

app.ConfigureSwagger();

// if (app.Environment.IsDevelopment())
// {
//     app.ConfigureSwagger();
// }

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();
app.MapControllers();

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