using Ardalis.GuardClauses;

namespace TgBotGuide.Domain.Entities;

public abstract class BaseEntity<T>
{
    /// <summary>
    /// Конструктор для сущности с определённым идентификатором
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    protected BaseEntity(
        T id
    )
    {
        Id = Guard.Against.Default(id, nameof(id));
        CreationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public T Id { get; }

    public DateTime CreationDate { get; private set; }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<T> entity)
        {
            return false;
        }

        if (ReferenceEquals(this, entity)) return true;

        return Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}