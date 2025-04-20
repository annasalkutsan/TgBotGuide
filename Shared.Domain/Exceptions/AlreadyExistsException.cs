namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее, когда объект уже существует
/// </summary>
public class AlreadyExistsException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="AlreadyExistsException"/> с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    protected AlreadyExistsException(string message)
        : base(message)
    {
    }
}