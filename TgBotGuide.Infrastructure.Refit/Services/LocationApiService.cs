using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Infrastructure.Refit.Interfaces;
using TgBotGuide.Infrastructure.Refit.Interfaces.Services;

namespace TgBotGuide.Infrastructure.Refit.Services;

public class LocationApiService : ILocationApiService
{
    private readonly ILocationApi _locationApi;

    public LocationApiService(ILocationApi locationApi)
    {
        _locationApi = locationApi;
    }

    public Task<List<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => _locationApi.GetAllAsync(cancellationToken);

    public Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _locationApi.GetByIdAsync(id, cancellationToken);

    public Task<List<LocationResponseDto>> GetByCityIdAsync(Guid cityId, CancellationToken cancellationToken = default)
        => _locationApi.GetByCityIdAsync(cityId, cancellationToken);

    public Task<List<LocationResponseDto>> GetByNameAsync(string locationName, CancellationToken cancellationToken = default)
    => _locationApi.GetByNameAsync(locationName, cancellationToken);
}