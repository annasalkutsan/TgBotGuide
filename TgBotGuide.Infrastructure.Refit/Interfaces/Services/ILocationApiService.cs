using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;

namespace TgBotGuide.Infrastructure.Refit.Interfaces.Services;

public interface ILocationApiService
{
    Task<List<LocationResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LocationResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<LocationResponseDto>> GetByCityIdAsync(Guid cityId, CancellationToken cancellationToken = default);
    Task<List<LocationResponseDto>> GetByNameAsync(string locationName, CancellationToken cancellationToken = default);
}