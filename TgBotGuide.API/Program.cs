using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

// Регистрация TelegramBotService с инъекцией зависимостей.
builder.Services.AddSingleton<TelegramBotService>(provider =>
{
    var options = provider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
    var botClient = new TelegramBotClient(options.Token);
    return new TelegramBotService(botClient, options);
});

// Добавляем контроллеры.
builder.Services.AddControllers();

// Добавляем Swagger для документации API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка подключения к базе данных.
var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options =>
{
    options.UseNpgsql(connectionString); // Используем PostgreSQL как СУБД.
});

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Добавляем AutoMapper для автоматического сопоставления объектов DTO.
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Добавляем валидацию через FluentValidation (раскомментируйте при необходимости).
// builder.Services.AddValidatorsFromAssemblyContaining<EmployeeRequestValidator>();

var app = builder.Build();

// Установка webhook для TelegramBot (вызов в начале жизненного цикла приложения).
using (var scope = app.Services.CreateScope())
{
    var telegramBotService = scope.ServiceProvider.GetRequiredService<TelegramBotService>();
    await telegramBotService.ConfigureWebhookAsync(); // Метод для установки webhook.
}

// Включаем Swagger в режиме разработки.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем перенаправление на HTTPS.
app.UseHttpsRedirection();

// Добавляем авторизацию (в данном коде отсутствует аутентификация, это placeholder).
app.UseAuthorization();

// Настраиваем маршрутизацию контроллеров.
app.MapControllers();

app.Run();