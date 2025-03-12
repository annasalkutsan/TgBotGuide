namespace TgBotGuide.Application.Dto;

public class LocationDto
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MapUrl { get; set; }
    public string ImageUrl { get; set; }
}