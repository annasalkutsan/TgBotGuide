namespace Shared.Application.Interfaces.Cache;

/// <summary>
/// Интерфейс сервиса для работы с кэшем
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Получить данные из кэша
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <typeparam name="T">Тип</typeparam>
    /// <returns>Найденный объект или null</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Записать данные в кэш
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="data">Данные</param>
    /// <param name="ttl">Время жизни данных в кэше</param>
    /// <typeparam name="T">Тип</typeparam>
    Task SetAsync<T>(string key, T data, TimeSpan ttl);

    /// <summary>
    /// Удалить данные из кэша
    /// </summary>
    /// <param name="key">Ключ</param>
    Task RemoveAsync(string key);
}