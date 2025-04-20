using Ardalis.GuardClauses;

namespace TgBotGuide.Domain.Entities;

public class Location : BaseEntity<Guid>
{
    private string _name;
    private string _description;
    private string _mapUrl;

    /// <summary>
    /// Конструктор локации
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cityId">Идентификатор города</param>
    /// <param name="name">Название локации</param>
    /// <param name="description">Описание локации</param>
    /// <param name="mapUrl">URL карты</param>
    /// <param name="imageUrl">URL изображения</param>
    public Location(Guid id, Guid cityId, string name, string description, string mapUrl, string? imageUrl) : base(id)
    {
        CityId = cityId;
        Name = name;
        Description = description;
        MapUrl = mapUrl;
        ImageUrl = imageUrl;
    }

    public Location() : base(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Идентификатор города
    /// </summary>
    public Guid CityId { get; private set; }

    /// <summary>
    /// Название локации
    /// </summary>
    public string Name
    {
        get => _name;
        private set => _name = Guard.Against.Null(value, nameof(value));
    }

    /// <summary>
    /// Описание локации
    /// </summary>
    public string Description
    {
        get => _description;
        private set => _description = Guard.Against.Null(value, nameof(value));
    }

    /// <summary>
    /// URL на карту, показывающую расположение
    /// </summary>
    public string MapUrl
    {
        get => _mapUrl;
        private set => _mapUrl = Guard.Against.Null(value, nameof(value));
    }

    /// <summary>
    /// URL изображения локации
    /// </summary>
    public string? ImageUrl { get; private set; }
}