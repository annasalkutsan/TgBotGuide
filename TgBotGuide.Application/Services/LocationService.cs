using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Shared.Application.Interfaces;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Interfaces.Repositories;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repository;
    private readonly MappingProfile _mapper; 
    private readonly IUnitOfWork _unitOfWork;

    public LocationService(
        ILocationRepository repository,
        MappingProfile mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    // Получение локации по ID
    public async Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id, false, cancellationToken);
        Guard.Against.Null(location, nameof(location)); // Проверка на null
        return _mapper.MapToLocationResponseDto(location); // Вручную маппируем
    }

    // Получение всех локаций
    public async Task<IReadOnlyCollection<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var locations = await _repository.GetAllAsync(cancellationToken);
        return locations.Select(location => _mapper.MapToLocationResponseDto(location)).ToList(); // Вручную маппируем для всех локаций
    }

    // Поиск локаций по условию
    public async Task<IReadOnlyCollection<LocationResponseDto>> FindAsync(Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken)
    {
        var locations = await _repository.FindAsync(predicate, cancellationToken);
        return locations.Select(location => _mapper.MapToLocationResponseDto(location)).ToList(); // Вручную маппируем для всех найденных локаций
    }

    // Добавление новой локации
    public async Task<LocationResponseDto> Add(LocationDto dto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(dto, nameof(dto)); // Проверка на null
        var location = _mapper.MapToLocation(dto, Guid.NewGuid()); // Вручную маппируем
         _repository.Add(location);
         await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.MapToLocationResponseDto(location); // Вручную маппируем после добавления
    }

    // Обновление локации
    public async Task<LocationResponseDto> Update(Guid id, LocationDto dto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(dto, nameof(dto)); // Проверка на null

        var location = _mapper.MapToLocation(dto, id); // Вручную маппируем
         _repository.Update(location);  // Используем асинхронный метод UpdateAsync
         await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.MapToLocationResponseDto(location); // Возвращаем обновленную локацию
    }

    // Удаление локации
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
         _repository.Remove(id);  // Используем асинхронный метод RemoveAsync
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}