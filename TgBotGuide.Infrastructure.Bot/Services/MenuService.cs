using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotGuide.Application.Interfaces.Bot;
using TgBotGuide.Infrastructure.Refit.Interfaces.Services;

namespace TgBotGuide.Infrastructure.Bot.Services;

public class MenuService : IMenuService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ICityApiService _cityService;
    private readonly ILocationApiService _locationService;

    private readonly Dictionary<long, int> _lastMessageIds = new();

    public MenuService(
        ITelegramBotClient botClient,
        ICityApiService cityService,
        ILocationApiService locationService)
    {
        _botClient = botClient;
        _cityService = cityService;
        _locationService = locationService;
    }

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
                Console.WriteLine($"Error deleting message: {ex.Message}");
            }
        }
    }

    public async Task ShowStartMenu(long chatId)
    {
        await DeleteLastMessageAsync(chatId);

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Выбрать город", "choose_city") },
            new[] { InlineKeyboardButton.WithCallbackData("О боте", "info") }
        });

        var sentMessage = await _botClient.SendTextMessageAsync(chatId,
            "Привет! Я помогу вам с выбором мест для посещения.",
            replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task ShowBotInfo(long chatId)
    {
        await DeleteLastMessageAsync(chatId);

        var infoText =
            "Этот бот помогает выбрать места для посещения в различных городах. Вы можете выбрать город и получить список интересных мест для посещения.";
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        });

        var sentMessage = await _botClient.SendTextMessageAsync(chatId, infoText, replyMarkup: inlineKeyboard);
        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task ShowCitySelection(long chatId)
    {
        await DeleteLastMessageAsync(chatId);

        var cities = await _cityService.GetAllAsync(CancellationToken.None);
        var inlineKeyboard = new InlineKeyboardMarkup(cities.Select(city =>
            new[] { InlineKeyboardButton.WithCallbackData(city.Name, $"city_{city.Id}") }
        ).ToArray());

        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Назад", "start_menu") }
        }).ToArray();

        var sentMessage = await _botClient.SendTextMessageAsync(chatId, "Выберите город:", replyMarkup: inlineKeyboard);
        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task ShowCityDetails(Guid cityId, long chatId)
    {
        var city = await _cityService.GetByIdAsync(cityId, CancellationToken.None);
        var locations = await _locationService.GetByCityIdAsync(cityId, CancellationToken.None);

        var inlineKeyboard = new InlineKeyboardMarkup(locations.Select(location =>
            new[] { InlineKeyboardButton.WithCallbackData(location.Name, $"location_{location.Name}") }
        ).ToArray());

        inlineKeyboard.InlineKeyboard = inlineKeyboard.InlineKeyboard.Concat(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Назад", "choose_city") }
        }).ToArray();

        var sentMessage = await _botClient.SendTextMessageAsync(chatId,
            $"Вы выбрали город {city.Name}. Вот места, которые мы советуем вам посетить:",
            replyMarkup: inlineKeyboard);

        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task ShowLocationDetails(Guid locationId, long chatId)
    {
        await DeleteLastMessageAsync(chatId);
        var location = await _locationService.GetByIdAsync(locationId, CancellationToken.None);

        var locationDetails = $"Название локации: {location.Name}\n" +
                              $"Описание: {location.Description}\n" +
                              $"Ссылка на Google Maps: {location.MapUrl}\n";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Назад", $"city_{location.CityId}") }
        });

        var sentMessage = await _botClient.SendPhotoAsync(
            chatId,
            location.ImageUrl,
            caption: locationDetails,
            replyMarkup: inlineKeyboard
        );

        _lastMessageIds[chatId] = sentMessage.MessageId;
    }

    public async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
    {
        var data = callbackQuery.Data;
        var chatId = callbackQuery.Message.Chat.Id;

        if (data.StartsWith("location_"))
        {
            var locationName = data.Substring(9);
            var locations = await _locationService.GetByNameAsync(locationName, CancellationToken.None);

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