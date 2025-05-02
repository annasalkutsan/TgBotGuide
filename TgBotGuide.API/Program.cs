using Microsoft.EntityFrameworkCore;
using TgBotGuide.API.Extensions;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Infrastructure.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddRepositories();
builder.Services.AddApplicationServices();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

builder.Services.AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/ping", () => "pong")
    .WithName("Ping")
    .WithTags("Check")
    .WithOpenApi();

app.MapControllers();

app.Run();