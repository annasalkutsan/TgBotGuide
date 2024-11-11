using System.Linq.Expressions;

namespace TgBotGuide.Application.Interfaces;

public interface ICrudService<TEntity, TDto, TResponseDto> 
    where TEntity : class
    where TDto : class
    where TResponseDto : class
{
    Task<TResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ICollection<TResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ICollection<TResponseDto>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TResponseDto> AddAsync(TDto dto, CancellationToken cancellationToken);
    Task<TResponseDto> UpdateAsync(Guid id, TDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}