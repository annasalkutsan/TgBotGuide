using System.Linq.Expressions;
using Shared.Application.Interfaces;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Interfaces.Repositories;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Application.Mapping;

namespace TgBotGuide.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _repository;
    private readonly MappingProfile _mappingProfile;
    private readonly IUnitOfWork _unitOfWork;

    public CityService(ICityRepository repository, MappingProfile mappingProfile, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mappingProfile = mappingProfile;
        _unitOfWork = unitOfWork;
    }

    // Получение города по ID
    public async Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(id, false, cancellationToken);
        return _mappingProfile.MapToCityResponseDto(city);
    }

    // Получение всех городов
    public async Task<IReadOnlyCollection<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cities = await _repository.GetAllAsync();
        return cities.Select(city => _mappingProfile.MapToCityResponseDto(city)).ToList();
    }

    // Поиск городов по условию
    public async Task<IReadOnlyCollection<CityResponseDto>> FindAsync(Expression<Func<City, bool>> predicate,
        CancellationToken cancellationToken)
    {
        var cities = await _repository.FindAsync(predicate);
        return cities.Select(city => _mappingProfile.MapToCityResponseDto(city)).ToList();
    }

    // Добавление нового города
    public async Task<CityResponseDto> Add(CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mappingProfile.MapToCity(dto, Guid.NewGuid()); // Используем уникальный ID для нового города
        _repository.Add(city);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mappingProfile.MapToCityResponseDto(city); // Возвращаем добавленный город
    }

    // Обновление города
    public async Task<CityResponseDto> Update(Guid id, CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mappingProfile.MapToCity(dto, id);
        _repository.Update(city);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mappingProfile.MapToCityResponseDto(city); // Возвращаем обновленный город
    }

    // Удаление города
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        _repository.Remove(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}