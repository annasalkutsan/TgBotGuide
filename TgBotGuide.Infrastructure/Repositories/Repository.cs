using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.Infrastructure.Repositories;

public class Repository<T>:IRepository<T> where T : class
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
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(ICollection<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public void RemoveRange(ICollection<T> entities)
    {
        _dbSet.RemoveRange(entities);
        _context.SaveChanges();
    }
}