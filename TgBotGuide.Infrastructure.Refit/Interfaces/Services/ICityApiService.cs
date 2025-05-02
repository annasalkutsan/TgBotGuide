using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;

namespace TgBotGuide.Infrastructure.Refit.Interfaces.Services;

public interface ICityApiService
{
    Task<List<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}