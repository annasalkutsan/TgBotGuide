namespace TgBotGuide.Application.Dto;

public class LocationDto
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string ImageUrl { get; set; }
    public List<Guid> CategoryIds { get; set; } // список ID категорий для связи
}