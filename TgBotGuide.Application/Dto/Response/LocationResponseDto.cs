namespace TgBotGuide.Application.Dto.Response;

public class LocationResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public AddressDto Address { get; set; }
    public string ImageUrl { get; set; }
    public List<CategoryDto> Categories { get; set; }

    public LocationResponseDto()
    {
        Categories = new List<CategoryDto>();
    }
}