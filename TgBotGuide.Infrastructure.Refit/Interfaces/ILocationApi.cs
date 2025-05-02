using Refit;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;

namespace TgBotGuide.Infrastructure.Refit.Interfaces;

public interface ILocationApi
{
    [Get("/api/location")]
    Task<List<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    [Get("/api/location/{id}")]
    Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/location/by-city/{cityId}")]
    Task<List<LocationResponseDto>> GetByCityIdAsync(Guid cityId, CancellationToken cancellationToken = default);
    
    [Get("/api/location/by-name/{locationName}")]
    Task<List<LocationResponseDto>> GetByNameAsync(string locationName, CancellationToken cancellationToken = default);
}