namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее, когда объект сущности уже существует
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class EntityAlreadyExistsException<T> : AlreadyExistsException
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="EntityAlreadyExistsException"/> с заданным сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public EntityAlreadyExistsException(string message) : base(message)
    {
    }
}