using System.Linq.Expressions;
using Shared.Domain;
using TgBotGuide.Domain.Entities;

namespace Shared.Application.Interfaces;

/// <summary>
/// Интерфейс репозитория 
/// </summary>
public interface IRepository<T> where T : BaseEntity<Guid>
{
    /// <summary>
    /// Получение по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <param name="tracking">Флаг для отслеживания изменений</param>
    /// <returns>Задача, результат которой, сущность соответсвующая идентификатору</returns>
    Task<T> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение всех сущностей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, результат которой, коллекция сущностей</returns>
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Поиск сущностей по условию предиката
    /// </summary>
    /// <param name="predicate">Предикат с параметром</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Коллекция сущностей соответствующих условию</returns>
    Task<IReadOnlyCollection<T>> FindAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default);

    /// Добавление сущности
    /// </summary>
    /// <param name="entity">Сущность, которую нужно добавить</param>
    /// <returns>Идентификатор добавленной сущности</returns>
    Guid Add(T entity);

    /// <summary>
    /// Обновление сущности
    /// </summary>
    /// <param name="entity">Сущность, которую нужно обновить</param>
    void Update(T entity);

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    /// <param name="entityId">Идентификатор квалификации</param>
    void Remove(Guid entityId);
}