using Microsoft.Extensions.Options; 
using Telegram.Bot; 
using Telegram.Bot.Types; 
using Telegram.Bot.Types.Enums; 
using Telegram.Bot.Types.ReplyMarkups; 
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.Application.Services;

public class TelegramBotService
{
    // Интерфейсы для получения данных о городах и локациях.
    private readonly ICityService _cityService;
    private readonly ILocationService _locationService;

    // Telegram Bot Client для взаимодействия с Telegram API.
    private readonly ITelegramBotClient _botClient;

    // Опции для бота, такие как токен и webhook URL.
    private readonly TelegramBotOptions _options;

    // Словарь для хранения состояния пользователей (например, выбранный город).
    private readonly Dictionary<long, string> _userState = new();
    
    public TelegramBotService(
        ITelegramBotClient botClient,
        IOptions<TelegramBotOptions> options, ICityService cityService, ILocationService locationService, TelegramBotOptions options1)
    {
        _botClient = botClient;
        _cityService = cityService;
        _locationService = locationService;
        _options = options.Value;
    }
    public TelegramBotService(ITelegramBotClient botClient, TelegramBotOptions options)
    {
        _botClient = botClient;
        _options = options;
    }
    
    public async Task ConfigureWebhookAsync()
    {
        try
        {
            var webhookUrl = _options.WebhookUrl; // URL для вебхука
            await _botClient.SetWebhookAsync(webhookUrl);
            Console.WriteLine($"Webhook установлен на: {webhookUrl}");
            var webhookInfo = await _botClient.GetWebhookInfoAsync();
            Console.WriteLine($"Webhook URL: {webhookInfo.Url}");
            Console.WriteLine($"Last error message: {webhookInfo.LastErrorMessage}");
            Console.WriteLine($"Last error date: {webhookInfo.LastErrorDate}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при настройке вебхука: {ex.Message}");
        }
    }

    // Метод для обработки входящих обновлений (например, сообщений или команд).
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Обновление получено: {update.Type}");

        if (update.Type == UpdateType.Message && update.Message is { Text: { } } message)
        {
            var chatId = message.Chat.Id;
            var userMessage = message.Text;
            Console.WriteLine($"Сообщение от пользователя {chatId}: {userMessage}");

            // Если пользователь выбрал город, показываем локации
            if (_userState.ContainsKey(chatId) && _userState[chatId] == "citySelected")
            {
                var selectedCity = _userState[chatId];

                switch (userMessage)
                {
                    case "Назад":
                        await ShowCities(chatId); // Возвращаемся к выбору города.
                        break;
                    default:
                        await ShowLocationInfo(chatId, userMessage); // Показываем информацию о выбранной локации.
                        break;
                }
            }
            else
            {
                // Обработка команд до выбора города
                switch (userMessage)
                {
                    case "/start":
                        await SendWelcomeMessage(chatId);
                        break;
                    case "Выбрать город":
                        await ShowCities(chatId);
                        break;
                    case "О боте":
                        await SendAboutMessage(chatId);
                        break;
                }
            }
        }
    }
    
    // Метод для отправки приветственного сообщения.
    private async Task SendWelcomeMessage(long chatId)
    {
        var buttons = new[]
        {
            new[] { new KeyboardButton("Выбрать город") },
            new[] { new KeyboardButton("О боте") }
        };
        var replyMarkup = new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true }; // Адаптивная клавиатура.

        await _botClient.SendTextMessageAsync(chatId, "Добро пожаловать! Выберите действие:", replyMarkup: replyMarkup);
    }

    // Метод для отправки информации о боте.
    private async Task SendAboutMessage(long chatId)
    {
        await _botClient.SendTextMessageAsync(chatId, "Этот бот помогает вам исследовать интересные места и достопримечательности Приднестровья. Выберите город, чтобы начать!");
    }

    // Метод для отображения доступных городов.
    private async Task ShowCities(long chatId)
    {
        var cities = await _cityService.GetAllAsync(CancellationToken.None); // Получаем список городов.

        if (!cities.Any()) // Если нет доступных городов.
        {
            await _botClient.SendTextMessageAsync(chatId, "Доступных городов пока нет.");
            return;
        }

        var buttons = cities
            .Select(city => new KeyboardButton(city.Name)) // Кнопки для каждого города.
            .Select(btn => new[] { btn })
            .Append(new[] { new KeyboardButton("Назад") }) // Кнопка возврата.
            .ToArray();

        var replyMarkup = new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(chatId, "Выберите город:", replyMarkup: replyMarkup);

        // Сохраняем состояние о выбранном городе.
        _userState[chatId] = "citySelected";
    }
    
    // Метод для отображения информации о локации.
    private async Task ShowLocationInfo(long chatId, string locationName)
    {
        // Получаем все локации и ищем нужную по названию
        var locations = await _locationService.GetAllAsync(CancellationToken.None);

        var selectedLocation = locations.FirstOrDefault(loc => loc.Name.Equals(locationName, StringComparison.OrdinalIgnoreCase));

        if (selectedLocation == null)
        {
            await _botClient.SendTextMessageAsync(chatId, "Локация не найдена.");
            return;
        }

        // Отправляем информацию о локации
        await _botClient.SendTextMessageAsync(chatId, $"Информация о локации {selectedLocation.Name}:\n\n{selectedLocation.Description}");
    }
    
    // Метод для отображения локаций для выбранного города.
    private async Task ShowLocations(long chatId)
    {
        var locations = await _locationService.GetAllAsync(CancellationToken.None); // Получаем все локации.

        if (!locations.Any()) // Если нет доступных локаций.
        {
            await _botClient.SendTextMessageAsync(chatId, "Доступных локаций пока нет.");
            return;
        }

        var buttons = locations
            .Select(location => new KeyboardButton(location.Name)) // Кнопки для каждой локации.
            .Select(btn => new[] { btn })
            .Append(new[] { new KeyboardButton("Назад") }) // Кнопка возврата.
            .ToArray();

        var replyMarkup = new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(chatId, "Выберите локацию:", replyMarkup: replyMarkup);
    }
    
    // Метод для возврата в предыдущее меню.
    private async Task HandleBackNavigation(long chatId)
    {
        if (_userState.ContainsKey(chatId)) // Если был выбран город.
        {
            await ShowCities(chatId); // Показываем города.
            _userState.Remove(chatId); // Убираем состояние.
        }
        else
        {
            await SendWelcomeMessage(chatId); // Показываем приветственное сообщение.
        }
    }
}
