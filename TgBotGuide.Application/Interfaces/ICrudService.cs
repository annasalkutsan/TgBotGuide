using System.Linq.Expressions;

namespace TgBotGuide.Application.Interfaces;

public interface ICrudService<TEntity, TDto, TResponseDto> 
    where TEntity : class
    where TDto : class
    where TResponseDto : class
{
    Task<TResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TResponseDto>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TResponseDto> Add(TDto dto, CancellationToken cancellationToken);
    Task<TResponseDto> Update(Guid id, TDto dto, CancellationToken cancellationToken);
    Task Remove (Guid id, CancellationToken cancellationToken);
}