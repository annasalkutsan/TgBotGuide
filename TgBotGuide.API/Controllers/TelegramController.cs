using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotGuide.Application.Services;

namespace TgBotGuide.Controllers;

[Route("api/telegram")]
[ApiController]
public class TelegramController : ControllerBase
{
    private readonly TelegramBotService _telegramBotService;
    private readonly ITelegramBotClient _botClient;

    public TelegramController(TelegramBotService telegramBotService, ITelegramBotClient botClient)
    {
        _telegramBotService = telegramBotService;
        _botClient = botClient;
    }

    // Этот метод будет вызываться Telegram при отправке обновлений (например, сообщений)
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] Update update, CancellationToken cancellationToken)
    {
        // Логируем полученные обновления для отладки
        Console.WriteLine($"Получено обновление: {update}");

        // Передаем обновление, botClient и cancellationToken в метод HandleUpdateAsync
        await _telegramBotService.HandleUpdateAsync(_botClient, update, cancellationToken);
        return Ok(); // Возвращаем успешный ответ
    }
}