using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.Application.Services;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMenuService _menuService;

    public TelegramBotService(ITelegramBotClient botClient, IMenuService menuService)
    {
        _botClient = botClient;
        _menuService = menuService;
    }

    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null)
        {
            var chatId = update.Message.Chat.Id;
            if (update.Message.Text == "/start")
            {
                await _menuService.ShowStartMenu(chatId);
            }
        }
        else if (update.CallbackQuery != null)
        {
            await _menuService.OnCallbackQueryReceived(update.CallbackQuery);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }
}