using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;
using Upload.API.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceConfiguration(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwagger();

builder.Services.AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseServiceConfiguration();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
}

app.UseHttpsRedirection();
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