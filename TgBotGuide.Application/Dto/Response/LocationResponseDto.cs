namespace TgBotGuide.Application.Dto.Response;

public class LocationResponseDto
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string ImageUrl { get; set; }
    public List<CategoryResponseDto> Categories { get; set; } // связанные категории
    public LocationResponseDto()
    {
        Categories = new List<CategoryResponseDto>();
    }
}