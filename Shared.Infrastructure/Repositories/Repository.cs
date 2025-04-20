using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Domain;
using Shared.Domain.Exceptions;
using TgBotGuide.Domain.Entities;


namespace Shared.Infrastructure.Repositories;

/// <summary>
/// Базовый репозиторий для операций чтения с сущностями типа T
/// </summary>
/// <typeparam name="T">Тип сущности, с которой работает репозиторий</typeparam>
public abstract class Repository<T> : IRepository<T> where T : BaseEntity<Guid>
{
    private readonly  DbContext _dbContext;
    protected IQueryable<T> Entities => _dbContext.Set<T>();

    /// <summary>
    /// Конструктор, инициализирующий репозиторий с контекстом базы данных
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    protected Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <param name="tracking">Флаг для отслеживания изменений</param>
    /// <returns>Задача, результат которой, сущность типа T или null, если сущность не найдена</returns>
    public async Task<T> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken = default)
    {
        var query = Entities.Where(e => e.Id == id);
        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        var entity = await query.SingleOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<T>(ExceptionsMessages.EntityNotFound(id));
        }

        return entity;
    }

    /// <summary>
    /// Получение всех сущностей типа T
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Задача, результат которой, список сущностей типа T</returns>
    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Entities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение сущностей типа T по условию
    /// </summary>
    /// <param name="predicate">Предикат с условием</param>
    /// <returns>Коллекция сущностей соответствующих условию</returns>
    public async Task<IReadOnlyCollection<T>> FindAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var query = Entities.Where(predicate);
        return await query.ToArrayAsync();
    }
    
    /// <summary>
    /// Метод для синхронного добавления новой сущности в базу данных
    /// </summary>
    /// <param name="entity">Сущность, которую нужно добавить</param>
    /// <returns>Идентификатор добавленной сущности</returns>
    public Guid Add(T entity)
    {
        _dbContext
            .Set<T>()
            .Add(entity);
        return entity.Id;
    }

    /// <summary>
    /// Метод для синхронного обновления сущности в базе данных
    /// </summary>
    /// <param name="entity">Сущность, которую нужно обновить</param>
    public void Update(T entity)
    {
        var existingEntity = _dbContext.Set<T>().Find(entity.Id);
        if (existingEntity == null)
        {
            throw new EntityNotFoundException<T>(ExceptionsMessages.EntityNotFound(entity.Id));
        }

        _dbContext.Set<T>().Update(entity);
    }

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    /// <param name="entityId">Идентификатор квалификации</param>
    public void Remove(Guid entityId)
    {
        Guard.Against.Null(entityId, nameof(entityId));

        var entity = _dbContext.Set<T>().Find(entityId);

        if (entity is null)
        {
            throw new EntityNotFoundException<T>(ExceptionsMessages.EntityNotFound(entityId));
        }

        _dbContext.Set<T>().Remove(entity);
    }
}