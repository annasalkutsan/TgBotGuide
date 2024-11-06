using Domain.ValueObjects;

namespace TgBotGuide.Domain.Entities;

public class Location:BaseEntity 
{
    public Guid CityId { get; set; }
    public City City { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public Address Address { get; set; }
    
    public string ImageUrl { get; set; }
    public ICollection<LocationCategory> LocationsCategories { get; set; }

    public Location()
    {
        LocationsCategories = new List<LocationCategory>();
    }
    
    public Location(Guid cityId, string name, string description, string coordinates, Address address, string imageUrl): this()
    {
        CityId = cityId;
        Name = name;
        Description = description;
        Coordinates = coordinates;
        Address = address;
        ImageUrl = imageUrl;
    }
}