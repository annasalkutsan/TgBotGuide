using System.Linq.Expressions;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;
using TgBotGuide.Application.Mapping;

namespace TgBotGuide.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _repository;
    private readonly MappingProfile _mappingProfile;

    public CityService(ICityRepository repository, MappingProfile mappingProfile)
    {
        _repository = repository;
        _mappingProfile = mappingProfile;
    }

    // Получение города по ID
    public async Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(id);
        return _mappingProfile.MapToCityResponseDto(city);
    }

    // Получение всех городов
    public async Task<ICollection<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cities = await _repository.GetAllAsync();
        return cities.Select(city => _mappingProfile.MapToCityResponseDto(city)).ToList();
    }

    // Поиск городов по условию
    public async Task<ICollection<CityResponseDto>> FindAsync(Expression<Func<City, bool>> predicate,
        CancellationToken cancellationToken)
    {
        var cities = await _repository.FindAsync(predicate);
        return cities.Select(city => _mappingProfile.MapToCityResponseDto(city)).ToList();
    }

    // Добавление нового города
    public async Task<CityResponseDto> AddAsync(CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mappingProfile.MapToCity(dto, Guid.NewGuid()); // Используем уникальный ID для нового города
        await _repository.AddAsync(city);
        return _mappingProfile.MapToCityResponseDto(city); // Возвращаем добавленный город
    }

    // Обновление города
    public async Task<CityResponseDto> UpdateAsync(Guid id, CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mappingProfile.MapToCity(dto, id); // Используем ID из параметра
        await _repository.UpdateAsync(city); // Используем асинхронный метод UpdateAsync
        return _mappingProfile.MapToCityResponseDto(city); // Возвращаем обновленный город
    }

    // Удаление города
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(id);
        if (city != null)
        {
            await _repository.RemoveAsync(city); // Используем асинхронный метод RemoveAsync
        }
    }
}