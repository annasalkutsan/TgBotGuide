using System.Linq.Expressions;
using AutoMapper;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _repository;
    private readonly IRepository<Location> _locationRepository; // Репозиторий для локаций
    private readonly IRepository<LocationCategory> _locationCategoryRepository; // Репозиторий для связи
    private readonly IMapper _mapper;

    public CategoryService(
        IRepository<Category> repository,
        IRepository<Location> locationRepository,
        IRepository<LocationCategory> locationCategoryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _locationRepository = locationRepository;
        _locationCategoryRepository = locationCategoryRepository;
        _mapper = mapper;
    }

    // Получение категории по ID
    public async Task<CategoryResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(id);
        return _mapper.Map<CategoryResponseDto>(category);
    }

    // Получение всех категорий
    public async Task<ICollection<CategoryResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync();
        return _mapper.Map<ICollection<CategoryResponseDto>>(categories);
    }

    // Поиск категорий по условию
    public async Task<ICollection<CategoryResponseDto>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
    {
        var categories = await _repository.FindAsync(predicate);
        return _mapper.Map<ICollection<CategoryResponseDto>>(categories);
    }

    // Добавление новой категории
    public async Task AddAsync(CategoryDto dto, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(dto);
        await _repository.AddAsync(category);
    }

    // Обновление категории
    public async Task UpdateAsync(Guid id, CategoryDto dto, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(dto);
        category.Id = id;
        _repository.Update(category);
    }

    // Удаление категории
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category != null)
        {
            _repository.Remove(category);
        }
    }

    // Добавление локации к категории
    public async Task AddLocationToCategoryAsync(Guid categoryId, Guid locationId, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(categoryId);
        var location = await _locationRepository.GetByIdAsync(locationId);

        if (category == null || location == null)
        {
            throw new ArgumentException("Category or Location not found.");
        }

        var locationCategory = new LocationCategory
        {
            LocationId = locationId,
            CategoryId = categoryId
        };

        await _locationCategoryRepository.AddAsync(locationCategory);
    }
}