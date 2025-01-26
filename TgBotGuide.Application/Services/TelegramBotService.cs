using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotGuide.Application.Interfaces;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ICityService _cityService;
    private readonly ILocationService _locationService;

    public TelegramBotService(ITelegramBotClient botClient, ICityService cityService, ILocationService locationService)
    {
        _botClient = botClient;
        _cityService = cityService;
        _locationService = locationService;
    }

    // Асинхронная функция для запуска polling
    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // Обработка всех типов обновлений
        };

        // Настроим обработчик обновлений
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        // Ожидаем остановки приложения (токен отмены)
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    // Обработчик обновлений
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message != null)
        {
            var chatId = update.Message.Chat.Id;

            // Обработка команды /start
            if (update.Message.Text == "/start")
            {
                await ShowStartMenu(chatId);
            }
        }
        else if (update.CallbackQuery != null)
        {
            // Обработка callback-запроса
            await OnCallbackQueryReceived(update.CallbackQuery);
        }
    }

    // Обработчик ошибок
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Логируем ошибку
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }    

    // Метод для отображения начального меню
    public async Task ShowStartMenu(long chatId)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Выбрать город", "choose_city") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("О боте", "info") }
        });

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Привет! Я помогу вам с выбором мест для посещения.",
            replyMarkup: inlineKeyboard
        );
    }

    // Метод для отображения информации о боте
    public async Task ShowBotInfo(long chatId)
    {
        var infoText = "Этот бот помогает выбрать места для посещения в различных городах. Вы можете выбрать город и получить список интересных мест для посещения.";
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        });

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: infoText,
            replyMarkup: inlineKeyboard
        );
    }

    // Метод для отображения выбора города
    public async Task ShowCitySelection(long chatId)
    {
        var cities = await _cityService.GetAllAsync(CancellationToken.None);
        var inlineKeyboard = new InlineKeyboardMarkup(cities.Select(city => 
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(city.Name, $"city_{city.Id}") }
        ).ToArray());

        // Добавляем кнопку "Назад" в меню выбора города
        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[] 
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        }).ToArray();

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите город:",
            replyMarkup: inlineKeyboard
        );
    }

    // Метод для отображения информации о выбранном городе
    public async Task ShowCityDetails(Guid cityId, long chatId)
    {
        var city = await _cityService.GetByIdAsync(cityId, CancellationToken.None);
        var locations = await _locationService.FindAsync(location => location.CityId == cityId, CancellationToken.None);

        var inlineKeyboard = new InlineKeyboardMarkup(locations.Select(location =>
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(location.Name, $"location_{location.Id}") }
        ).ToArray());

        // Добавляем кнопку "Назад" в меню с местами
        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[] 
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "choose_city") }
        }).ToArray();

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Вы выбрали город {city.Name}. Вот места, которые мы советуем вам посетить.",
            replyMarkup: inlineKeyboard
        );
    }

    // Метод для отображения информации о выбранном месте
    public async Task ShowLocationDetails(Guid locationId, long chatId)
    {
        var location = await _locationService.GetByIdAsync(locationId, CancellationToken.None);

        var locationDetails = $"{location.Name}\n{location.Description}\n" +
                              $"Ссылка на Google Maps: {location.MapUrl}\n";
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", $"city_{location.CityId}") }
        });

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: locationDetails,
            replyMarkup: inlineKeyboard
        );
    }

    // Обработчик callback-запросов
    public async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
    {
        var data = callbackQuery.Data;
        var chatId = callbackQuery.Message.Chat.Id;

        if (data.StartsWith("city_"))
        {
            var cityId = Guid.Parse(data.Substring(5));
            await ShowCityDetails(cityId, chatId);
        }
        else if (data == "start_menu")
        {
            await ShowStartMenu(chatId);
        }
        else if (data == "choose_city")
        {
            await ShowCitySelection(chatId);
        }
        else if (data.StartsWith("location_"))
        {
            var locationId = Guid.Parse(data.Substring(10));
            await ShowLocationDetails(locationId, chatId);
        }
        else if (data == "info")
        {
            await ShowBotInfo(chatId);
        }
    }
}