namespace TgBotGuide.Infrastructure.Migrator;

/// <summary>
/// Исключение, выбрасываемое, когда строка подключения не найдена в переменных окружения.
/// </summary>
public class MissingConnectionStringException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MissingConnectionStringException"/>.
    /// </summary>
    /// <remarks>
    /// Это исключение генерируется, когда строка подключения отсутствует в переменных окружения.
    /// </remarks>
    public MissingConnectionStringException()
        : base("Строка подключения не найдена в переменных окружения")
    {
    }
}