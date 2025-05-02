using Microsoft.Extensions.Options;
using Telegram.Bot;
using TgBotGuide.Application.Interfaces.Bot;
using TgBotGuide.Infrastructure.Bot.Options;
using TgBotGuide.Infrastructure.Bot.Services;
using TgBotGuide.Infrastructure.Refit.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddRefit(builder.Configuration);

builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(nameof(TelegramBotOptions)));
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

var app = builder.Build();

// Запускаем polling для получения обновлений от Telegram
using (var scope = app.Services.CreateScope())
{
    var telegramBotService = scope.ServiceProvider.GetRequiredService<TelegramBotService>();
    var cancellationToken = app.Lifetime.ApplicationStopping;
    await telegramBotService.StartPollingAsync(cancellationToken);
}

app.MapGet("/", () => "Hello World!");

app.Run();