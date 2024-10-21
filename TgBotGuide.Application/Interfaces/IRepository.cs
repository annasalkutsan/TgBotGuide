using System.Linq.Expressions;

namespace TgBotGuide.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<ICollection<T>> GetAllAsync();
    Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(ICollection<T> entities);
}