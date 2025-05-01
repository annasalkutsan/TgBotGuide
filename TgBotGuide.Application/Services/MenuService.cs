using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.Application.Services;

public class MenuService : IMenuService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ICityService _cityService;
    private readonly ILocationService _locationService;

    // Храним ID последних сообщений
    private readonly Dictionary<long, int> _lastMessageIds = new();

    public MenuService(ITelegramBotClient botClient, ICityService cityService, ILocationService locationService)
    {
        _botClient = botClient;
        _cityService = cityService;
        _locationService = locationService;
    }

    // Метод для удаления старого сообщения
    private async Task DeleteLastMessageAsync(long chatId)
    {
        if (_lastMessageIds.ContainsKey(chatId))
        {
            var messageId = _lastMessageIds[chatId];
            try
            {
                await _botClient.DeleteMessageAsync(chatId, messageId);
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если сообщение не удалось удалить (например, оно было уже удалено).
                Console.WriteLine($"Error deleting message: {ex.Message}");
            }
        }
    }

    public async Task ShowStartMenu(long chatId)
    {
        await DeleteLastMessageAsync(chatId); // Удаляем старое сообщение перед отправкой нового

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Выбрать город", "choose_city") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("О боте", "info") }
        });

        var sentMessage = await _botClient.SendTextMessageAsync(chatId,
            "Привет! Я помогу вам с выбором мест для посещения.",
            replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId; // Сохраняем ID нового сообщения
    }

    public async Task ShowBotInfo(long chatId)
    {
        await DeleteLastMessageAsync(chatId); // Удаляем старое сообщение перед отправкой нового

        var infoText =
            "Этот бот помогает выбрать места для посещения в различных городах. Вы можете выбрать город и получить список интересных мест для посещения.";
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        });

        var sentMessage = await _botClient.SendTextMessageAsync(chatId, infoText, replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId; // Сохраняем ID нового сообщения
    }

    public async Task ShowCitySelection(long chatId)
    {
        await DeleteLastMessageAsync(chatId); // Удаляем старое сообщение перед отправкой нового

        var cities = await _cityService.GetAllAsync(CancellationToken.None);
        var inlineKeyboard = new InlineKeyboardMarkup(cities.Select(city =>
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(city.Name, $"city_{city.Id}") }
        ).ToArray());

        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        }).ToArray();

        var sentMessage = await _botClient.SendTextMessageAsync(chatId, "Выберите город:", replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId; // Сохраняем ID нового сообщения
    }

    public async Task ShowCityDetails(Guid cityId, long chatId)
    {
        // Убираем удаление сообщения, так как это подробности города
        var city = await _cityService.GetByIdAsync(cityId, CancellationToken.None);
        var locations = await _locationService.FindAsync(location => location.CityId == cityId, CancellationToken.None);

        var inlineKeyboard = new InlineKeyboardMarkup(locations.Select(location =>
            new InlineKeyboardButton[]
                { InlineKeyboardButton.WithCallbackData(location.Name, $"location_{location.Name}") }
        ).ToArray());

        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", "choose_city") }
        }).ToArray();

        var sentMessage = await _botClient.SendTextMessageAsync(chatId,
            $"Вы выбрали город {city.Name}. Вот места, которые мы советуем вам посетить:", replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId; // Сохраняем ID нового сообщения
    }

    public async Task ShowLocationDetails(Guid locationId, long chatId)
    {
        await DeleteLastMessageAsync(chatId);
        var location = await _locationService.GetByIdAsync(locationId, CancellationToken.None);

        var locationDetails = $"Название локации: {location.Name}\n" +
                              $"Описание: {location.Description}\n" +
                              $"Ссылка на Google Maps: {location.MapUrl}\n";

        // Отправка изображения по URL
        var imageUrl = location.ImageUrl;

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Назад", $"city_{location.CityId}") }
        });

        // Отправляем сначала изображение, а затем текст с описанием
        var sentMessage = await _botClient.SendPhotoAsync(
            chatId,
            imageUrl,
            caption: locationDetails, // Добавляем описание
            replyMarkup: inlineKeyboard
        );

        // Запоминаем ID сообщения
        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
    {
        var data = callbackQuery.Data;
        var chatId = callbackQuery.Message.Chat.Id;

        if (data.StartsWith("location_"))
        {
            var locationName = data.Substring(9);
            var locations = await _locationService.FindAsync(l => l.Name == locationName, CancellationToken.None);

            if (locations.Any())
            {
                var location = locations.First();
                await ShowLocationDetails(location.Id, chatId);
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, "Локация не найдена.");
            }
        }
        else if (data == "choose_city")
        {
            await ShowCitySelection(chatId);
        }
        else if (data.StartsWith("city_"))
        {
            var cityId = Guid.Parse(data.Substring(5));
            await ShowCityDetails(cityId, chatId);
        }
        else if (data.StartsWith("info"))
        {
            await ShowBotInfo(chatId);
        }
        else if (data == "start_menu")
        {
            await ShowStartMenu(chatId);
        }
    }
}