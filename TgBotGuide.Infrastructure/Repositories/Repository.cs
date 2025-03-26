using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Infrastructure.Repositories;

public abstract class Repository<T>:IRepository<T> where T : class
{
    private readonly TgBotGuideDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(TgBotGuideDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<ICollection<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}