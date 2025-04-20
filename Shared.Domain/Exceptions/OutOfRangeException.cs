namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее при выходе значения за пределы допустимого диапазона.
/// </summary>
public class OutOfRangeException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения <see cref="OutOfRangeException"/> с дополнительной информацией
    /// </summary>
    /// <param name="propertyName">Имя свойства, значение которого выходит за пределы диапазона</param>
    /// <param name="value">Значение свойства</param>
    /// <param name="message">Сообщение об ошибке</param>
    protected OutOfRangeException(string propertyName, object value, string message)
        : base(message)
    {
        PropertyName = propertyName;
        Value = value;
    }

    /// <summary>
    /// Имя свойства, значение которого выходит за пределы диапазона
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Значение свойства, которое находится вне допустимого диапазона
    /// </summary>
    public object Value { get; }
}