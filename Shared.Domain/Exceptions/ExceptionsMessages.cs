namespace Shared.Domain.Exceptions;

public class ExceptionsMessages
{
    /// <summary>
    /// Генерирует сообщение исключения для случая, когда сущность с указанным Id не найдена.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Сообщение, которое будет использовано в исключении</returns>
    public static string EntityNotFound(Guid id) =>
        $"Сущность с идентификатором \"{id}\" не найдена.";
}