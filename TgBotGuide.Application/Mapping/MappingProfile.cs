using Ardalis.GuardClauses;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Application.Mapping;

public class MappingProfile
{
    public CityResponseDto MapToCityResponseDto(City city)
    {
        Guard.Against.Null(city, nameof(city));

        return new CityResponseDto
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description
        };
    }

    public City MapToCity(CityDto cityDto, Guid id)
    {
        Guard.Against.Null(cityDto, nameof(cityDto));

        return new City(id, cityDto.Name, cityDto.Description);
    }

    public LocationResponseDto MapToLocationResponseDto(Location location)
    {
        Guard.Against.Null(location, nameof(location));

        return new LocationResponseDto
        {
            Id = location.Id,
            CityId = location.CityId,
            Name = location.Name,
            Description = location.Description,
            MapUrl = location.MapUrl,
            ImageUrl = location.ImageUrl
        };
    }

    public Location MapToLocation(LocationDto locationDto, Guid id)
    {
        Guard.Against.Null(locationDto, nameof(locationDto));

        return new Location(id, locationDto.CityId, locationDto.Name, locationDto.Description, locationDto.MapUrl,
            locationDto.ImageUrl);
    }
}