namespace TgBotGuide.Domain.Entities;

public class Location:BaseEntity 
{
    public Guid CityId { get; set; }
    public City City { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Coordinates { get; set; }
    public string Address { get; set; }
    public ICollection<LocationCategory> LocationsCategories { get; set; }

    public Location()
    {
        LocationsCategories = new List<LocationCategory>();
    }
    
    public Location(Guid cityId, string name, string description, string coordinates, string address): this()
    {
        CityId = cityId;
        Name = name;
        Description = description;
        Coordinates = coordinates;
        Address = address;
    }
}