using System.Linq.Expressions;

namespace TgBotGuide.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<ICollection<T>> GetAllAsync();
    Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
}