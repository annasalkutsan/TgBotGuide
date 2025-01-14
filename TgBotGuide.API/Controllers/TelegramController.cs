using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TgBotGuide.Application.Services;
using Telegram.Bot.Types;

namespace TgBotGuide.Controllers
{
    [Route("api/telegram")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramBotService _telegramBotService;

        public TelegramController(TelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        // Этот метод будет вызываться Telegram при отправке обновлений (например, сообщений)
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            Console.WriteLine($"Получено обновление: {update}");
            await _telegramBotService.HandleUpdateAsync(update, CancellationToken.None);
            return Ok();
        }
    }
}