using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Application.Interfaces;
using Shared.Infrastructure;
using Telegram.Bot;
using TgBotGuide.Application;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Interfaces.Repositories;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Application.Services;
using TgBotGuide.Infrastructure;
using TgBotGuide.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Настройка подключения к базе данных.
var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options =>
{
    options.UseNpgsql(connectionString); 
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<TgBotGuideDbContext>>();

builder.Services.AddScoped<MappingProfile>();

builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Регистрация конфигурации для TelegramBotOptions
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection("TelegramBot"));

// Регистрация ITelegramBotClient как Singleton с использованием токена
builder.Services.AddSingleton<ITelegramBotClient>(provider =>
{
    var options = provider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
    return new TelegramBotClient(options.Token);
});

// Регистрация TelegramBotService с зависимостями
builder.Services.AddScoped<TelegramBotService>(provider =>
{
    var botClient = provider.GetRequiredService<ITelegramBotClient>();
    return new TelegramBotService(botClient, provider.GetRequiredService<IMenuService>());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Запускаем polling для получения обновлений от Telegram
using (var scope = app.Services.CreateScope())
{
    var telegramBotService = scope.ServiceProvider.GetRequiredService<TelegramBotService>();
    var cancellationToken = app.Lifetime.ApplicationStopping;
    await telegramBotService.StartPollingAsync(cancellationToken);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();