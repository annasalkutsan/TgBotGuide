namespace TgBotGuide.Application.Dto.Response;

public record CityResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}