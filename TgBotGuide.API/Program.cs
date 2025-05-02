using Microsoft.EntityFrameworkCore;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Application.Services;
using TgBotGuide.Domain.Interfaces;
using TgBotGuide.Infrastructure.DataBase;
using TgBotGuide.Infrastructure.DataBase.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Настройка подключения к базе данных.
var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Добавляем AutoMapper для автоматического сопоставления объектов DTO.
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Добавление контроллеров
builder.Services.AddControllers();

// Добавление Swagger и OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка Swagger для режима разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Маршрут для проверки
app.MapGet("/api/ping", () => "pong")
    .WithName("Ping")
    .WithTags("Check")
    .WithOpenApi();

// Добавление маршрутов для контроллеров
app.MapControllers();

app.Run();