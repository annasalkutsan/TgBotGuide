using Ardalis.GuardClauses;

namespace TgBotGuide.Domain.ValueObjects;

/// <summary>
/// Объект значения, представляющий адрес.
/// </summary>
public class Address : BaseValueObject
{
    private string _street;
    private string _house;

    /// <summary>
    /// Конструктор для инициализации адреса с валидацией.
    /// </summary>
    /// <param name="street">Улица.</param>
    /// <param name="house">Номер дома.</param>
    public Address(string street, string house)
    {
        Street = street;
        House = house;
    }

    public Address()
    {
    }

    /// <summary>
    /// Улица.
    /// </summary>
    public string Street
    {
        get => _street;
        private set => _street = Guard.Against.NullOrEmpty(value, nameof(value));
    }

    /// <summary>
    /// Номер дома.
    /// </summary>
    public string House
    {
        get => _house;
        private set => _house = Guard.Against.NullOrEmpty(value, nameof(value));
    }
}