namespace Shared.Application.Interfaces;

/// <summary>
/// Интерфейс для управления транзакциями и изменениями в базе данных
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет все изменения, выполненные в репозиториях
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, результат которой, количество записей при сохранении в базу данных</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}