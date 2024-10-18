namespace TgBotGuide.Domain.Entities;

public class City:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Location> Locations { get; set; }

    public City()
    {
        Locations= new List<Location>();
    }

    public City(string name, string description): this()
    {
        Name = name;
        Description = description;
    }
}