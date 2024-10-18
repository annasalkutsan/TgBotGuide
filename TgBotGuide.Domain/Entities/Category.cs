namespace TgBotGuide.Domain.Entities;

public class Category:BaseEntity
{
    public string Name { get; set; }
    public ICollection<LocationCategory> LocationsCategories { get; set; }

    public Category()
    {
        LocationsCategories = new List<LocationCategory>();
    }

    public Category(string name): this()
    {
        Name = name;
    }
}