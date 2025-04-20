namespace Shared.Domain.Exceptions;

/// <summary>
/// /// Исключение, которое выбрасывается, когда возникает конфликт индексов
/// </summary>
public class IndexConflictException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="IndexConflictException"/> с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public IndexConflictException(string message)
        : base(message)
    {
    }
}