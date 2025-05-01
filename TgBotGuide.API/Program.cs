using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Telegram.Bot;
using TgBotGuide.Application;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Application.Services;
using TgBotGuide.Domain.Interfaces;
using TgBotGuide.Infrastructure;
using TgBotGuide.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Чтение конфигурации из appsettings.json.
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection("TelegramBot"));
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddSingleton<ITelegramBotClient>(provider =>
{
    var options = provider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
    return new TelegramBotClient(options.Token);
});
builder.Services.AddScoped<TelegramBotService>(provider =>
{
    var botClient = provider.GetRequiredService<ITelegramBotClient>();
    return new TelegramBotService(botClient, provider.GetRequiredService<IMenuService>());
});

// Добавляем контроллеры.
builder.Services.AddControllers();

// Добавляем Swagger для документации API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Skills.Api",
        Description = "API для работы с сущностью Skill  в рамках проекта StaffPro",
        Version = "v1",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    //options.EnableAnnotations();
    //options.ExampleFilters();
});

//builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());


// Настройка подключения к базе данных.
var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Добавляем AutoMapper для автоматического сопоставления объектов DTO.
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Запускаем polling для получения обновлений от Telegram
using (var scope = app.Services.CreateScope())
{
    var telegramBotService = scope.ServiceProvider.GetRequiredService<TelegramBotService>();
    var cancellationToken = app.Lifetime.ApplicationStopping;
    await telegramBotService.StartPollingAsync(cancellationToken);
}

// Настраиваем маршрутизацию контроллеров.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/ping", () => "pong")
    .WithName("Ping")
    .WithTags("Check");

app.MapControllers();

app.Run();