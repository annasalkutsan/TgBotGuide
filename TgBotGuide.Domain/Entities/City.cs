using Ardalis.GuardClauses;
using Shared.Domain;

namespace TgBotGuide.Domain.Entities;

public class City : BaseEntity<Guid>
{
    private string _name;
    private string _description;

    /// <summary>
    /// Конструктор для создания города с именем и описанием
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="name">Название города</param>
    /// <param name="description">Описание города</param>
    public City(Guid id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public City() : base(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Название города
    /// </summary>
    public string Name
    {
        get => _name;
        private set => _name = Guard.Against.Null(value, nameof(value));
    }

    /// <summary>
    /// Описание города
    /// </summary>
    public string Description
    {
        get => _description;
        private set => _description = Guard.Against.Null(value, nameof(value));
    }

    /// <summary>
    /// Локации, связанные с городом
    /// </summary>
    public IReadOnlyCollection<Location> Locations { get; private set; } = new List<Location>();
}