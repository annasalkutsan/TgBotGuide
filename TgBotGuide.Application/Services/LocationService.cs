using System.Linq.Expressions;
using AutoMapper;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services;

public class LocationService : ILocationService
{
    private readonly IRepository<Location> _repository;
    private readonly IRepository<LocationCategory> _locationCategoryRepository; // Репозиторий для связи
    private readonly IRepository<Category> _categoryRepository; // Репозиторий для категорий
    private readonly IMapper _mapper;

    public LocationService(
        IRepository<Location> repository,
        IRepository<LocationCategory> locationCategoryRepository,
        IRepository<Category> categoryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _locationCategoryRepository = locationCategoryRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    // Получение локации по ID
    public async Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id);
        return _mapper.Map<LocationResponseDto>(location);
    }

    // Получение всех локаций
    public async Task<ICollection<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var locations = await _repository.GetAllAsync();
        return _mapper.Map<ICollection<LocationResponseDto>>(locations);
    }

    // Поиск локаций по условию
    public async Task<ICollection<LocationResponseDto>> FindAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken)
    {
        var locations = await _repository.FindAsync(predicate);
        return _mapper.Map<ICollection<LocationResponseDto>>(locations);
    }

    // Добавление новой локации
    public async Task AddAsync(LocationDto dto, CancellationToken cancellationToken)
    {
        var location = _mapper.Map<Location>(dto);
        await _repository.AddAsync(location);
    }

    // Обновление локации
    public async Task UpdateAsync(Guid id, LocationDto dto, CancellationToken cancellationToken)
    {
        var location = _mapper.Map<Location>(dto);
        location.Id = id;
        _repository.Update(location);
    }

    // Удаление локации
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location != null)
        {
            _repository.Remove(location);
        }
    }

    // Добавление категории к локации
    public async Task AddCategoryToLocationAsync(Guid locationId, Guid categoryId, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(locationId);
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (location == null || category == null)
        {
            throw new ArgumentException("Location or Category not found.");
        }

        var locationCategory = new LocationCategory
        {
            LocationId = locationId,
            CategoryId = categoryId
        };

        await _locationCategoryRepository.AddAsync(locationCategory);
    }
}