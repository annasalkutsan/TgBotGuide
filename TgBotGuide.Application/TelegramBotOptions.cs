namespace TgBotGuide.Application;

public class TelegramBotOptions
{
    public string Token { get; set; }      // Токен бота, получаемый от Telegram.
    public string WebhookUrl { get; set; } // URL для настройки вебхука.
}