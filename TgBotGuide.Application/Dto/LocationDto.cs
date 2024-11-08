namespace TgBotGuide.Application.Dto;

public class LocationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public AddressDto Address { get; set; }
    public string ImageUrl { get; set; }
    public List<Guid> CategoryIds { get; set; } // Список Id категорий для локации
}