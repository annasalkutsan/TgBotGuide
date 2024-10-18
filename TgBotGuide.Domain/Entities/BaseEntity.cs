namespace TgBotGuide.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity entity)
        {
            return false;
        }

        if (ReferenceEquals(this, entity)) return true;

        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}