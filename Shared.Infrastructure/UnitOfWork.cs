using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;

namespace Shared.Infrastructure;

/// <summary>
/// Класс для работы с сохранением изменений в контексте базы данных
/// </summary>
public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    /// <summary>
    /// Конструктор, инициализирующий UnitOfWork с контекстом данных
    /// </summary>
    /// <param name="dbContext">Контекст данных, с которым будет работать UnitOfWork</param>
    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext), $"\"{nameof(dbContext)}\" не может быть null.");
    }

    /// <summary>
    /// Метод для сохранения всех изменений, выполненных в репозиториях, в базе данных
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Задача, результат которой, количество записей при сохранении в базу данных</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}