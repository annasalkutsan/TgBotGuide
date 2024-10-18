namespace TgBotGuide.Domain.Entities;

public class LocationCategory
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    
    public LocationCategory(){}

    public LocationCategory(Guid locationId, Guid categoryId)
    {
        LocationId = locationId;
        CategoryId = categoryId;
    }
}