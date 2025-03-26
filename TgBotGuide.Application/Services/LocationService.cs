using System.Linq.Expressions;
using Ardalis.GuardClauses;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repository;
    private readonly MappingProfile _mapper; 

    public LocationService(
        ILocationRepository repository,
        MappingProfile mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // Получение локации по ID
    public async Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id);
        Guard.Against.Null(location, nameof(location)); // Проверка на null
        return _mapper.MapToLocationResponseDto(location); // Вручную маппируем
    }

    // Получение всех локаций
    public async Task<ICollection<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var locations = await _repository.GetAllAsync();
        return locations.Select(location => _mapper.MapToLocationResponseDto(location)).ToList(); // Вручную маппируем для всех локаций
    }

    // Поиск локаций по условию
    public async Task<ICollection<LocationResponseDto>> FindAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken)
    {
        var locations = await _repository.FindAsync(predicate);
        return locations.Select(location => _mapper.MapToLocationResponseDto(location)).ToList(); // Вручную маппируем для всех найденных локаций
    }

    // Добавление новой локации
    public async Task<LocationResponseDto> AddAsync(LocationDto dto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(dto, nameof(dto)); // Проверка на null
        var location = _mapper.MapToLocation(dto, Guid.NewGuid()); // Вручную маппируем
        await _repository.AddAsync(location);
        return _mapper.MapToLocationResponseDto(location); // Вручную маппируем после добавления
    }

    // Обновление локации
    public async Task<LocationResponseDto> UpdateAsync(Guid id, LocationDto dto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(dto, nameof(dto)); // Проверка на null

        var location = _mapper.MapToLocation(dto, id); // Вручную маппируем
        await _repository.UpdateAsync(location);  // Используем асинхронный метод UpdateAsync
        return _mapper.MapToLocationResponseDto(location); // Возвращаем обновленную локацию
    }

    // Удаление локации
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id);
        Guard.Against.Null(location, nameof(location)); // Проверка на null
        await _repository.RemoveAsync(location);  // Используем асинхронный метод RemoveAsync
    }
}