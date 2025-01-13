using BuildingBlocks.Logging;
using BuildingBlocks.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddSerilogService(builder.Configuration, builder.Host);



var app = builder.Build();

app.UseMiddleware<IpLoggingMiddleware>();

// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
app.MapReverseProxy();

try
{
    Log.Information("Application starting");
    await app.RunAsync();
    Log.Information("Application started");
}
catch(Exception e)
{
    Log.Fatal(e, "Application start Failed");
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Program { }