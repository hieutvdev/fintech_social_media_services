
using Auth.API.Middlewares;
using Auth.Application.DependencyInjection.Extensions;
using Auth.Infrastructure.DependencyInjection.Extensions;
using BuildingBlocks.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the bearer token Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});
//builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);

builder.Services.AddSerilogService(builder.Configuration, builder.Host);
builder.Services
    .AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration);





var app = builder.Build();

app.UseApplicationService();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthMiddlewareV1>();
app.MapControllers();

app.Run();
