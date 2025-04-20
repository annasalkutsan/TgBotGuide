namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее при выходе значения за пределы допустимого диапазона.
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class EntityOutOfRangeException<T> : OutOfRangeException
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="EntityOutOfRangeException"/> с дополнительной информацией
    /// </summary>
    /// <param name="propertyName">Имя свойства, значение которого выходит за пределы диапазона</param>
    /// <param name="value">Значение свойства</param>
    /// <param name="message">Сообщение об ошибке</param>
    protected EntityOutOfRangeException(string propertyName, object value, string message) : base(propertyName, value, message)
    {
    }
}