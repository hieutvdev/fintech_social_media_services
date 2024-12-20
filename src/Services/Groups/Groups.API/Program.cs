using BuildingBlocks.Logging;
using Groups.API.DependencyInjection.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenOptions();
builder.Services.AddSerilogService(builder.Configuration, builder.Host);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


try
{
    Log.Information("Application Starting");
    await app.RunAsync();
    Log.Information("Application Started");
}
catch (Exception e)
{
    Log.Fatal(e, "Application start Failed");
}finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}public partial class Program { }