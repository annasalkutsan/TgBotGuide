namespace TgBotGuide.Application.Dto.Response;

public record LocationResponseDto
{
    public Guid Id { get; init; }
    public Guid CityId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string MapUrl { get; init; }
    public string? ImageUrl { get; init; }
}