namespace TgBotGuide.Application.Dto.Response;

public class LocationResponseDto
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MapUrl { get; set; }
    public string ImageUrl { get; set; }

}