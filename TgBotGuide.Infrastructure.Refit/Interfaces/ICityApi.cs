using Refit;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;

namespace TgBotGuide.Infrastructure.Refit.Interfaces;

public interface ICityApi
{
    [Get("/api/city")]
    Task<List<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    [Get("/api/city/{id}")]
    Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}