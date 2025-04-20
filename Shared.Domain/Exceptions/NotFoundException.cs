namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее, когда объект не найден
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="NotFoundException"/> с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    protected NotFoundException(string message)
        : base(message)
    {
    }
}