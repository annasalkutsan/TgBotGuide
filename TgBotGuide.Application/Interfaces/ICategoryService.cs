using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Application.Interfaces;

public interface ICategoryService : ICrudService<Category, CategoryDto, CategoryResponseDto>
{
    Task AddLocationToCategoryAsync(Guid categoryId, Guid locationId, CancellationToken cancellationToken);
}
