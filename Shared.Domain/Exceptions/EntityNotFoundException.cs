namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее, когда объект сущности не найден
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class EntityNotFoundException<T> : NotFoundException
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="EntityNotFoundException"/> с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public EntityNotFoundException(string message) : base(message)
    {
    }
}