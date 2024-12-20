using BuildingBlocks.Logging;
using Mail.API.DependencyInjection.Extensions;
using Mail.API.Installers;
using Mail.API.Interfaces;
using Mail.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.InstallerServicesInAssembly(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddSerilogService(builder.Configuration, builder.Host);
builder.Services.AddAuthorization();
var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     await Task.Delay(TimeSpan.FromSeconds(10));
//     var sendMailService = scope.ServiceProvider.GetRequiredService<ISendMailService>();
//     await sendMailService.StartConsumingAsync();
// }
app.UseService();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.Run();
