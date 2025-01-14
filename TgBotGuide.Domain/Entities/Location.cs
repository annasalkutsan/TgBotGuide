using Domain.ValueObjects;

namespace TgBotGuide.Domain.Entities;

public class Location:BaseEntity 
{
    public Guid CityId { get; set; }
    public City City { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MapUrl { get; set; }
    public string ImageUrl { get; set; }
    
    public Location(Guid cityId, string name, string description, string mapUrl, string imageUrl)
    {
        CityId = cityId;
        Name = name;
        Description = description;
        MapUrl = mapUrl;
        ImageUrl = imageUrl;
    }
}