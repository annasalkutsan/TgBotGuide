using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Infrastructure.Refit.Interfaces;
using TgBotGuide.Infrastructure.Refit.Interfaces.Services;

namespace TgBotGuide.Infrastructure.Refit.Services;

public class CityApiService : ICityApiService
{
    private readonly ICityApi _cityApi;

    public CityApiService(ICityApi cityApi)
    {
        _cityApi = cityApi;
    }

    public Task<List<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => _cityApi.GetAllAsync(cancellationToken);

    public Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _cityApi.GetByIdAsync(id, cancellationToken);
}