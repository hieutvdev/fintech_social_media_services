using Article.Application.DependencyInjection.Extensions;
using Article.Infrastructure.DependencyInjection.Extensions;
using BuildingBlocks.Logging;
using BuildingBlocks.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilogService(builder.Configuration, builder.Host);

builder.Services.AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration);


var app = builder.Build();
app.UseMiddleware<IpLoggingMiddleware>();
app.UseApplicationService();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

try
{

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    
}
app.Run();

