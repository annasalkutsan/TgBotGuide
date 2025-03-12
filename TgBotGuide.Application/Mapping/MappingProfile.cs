using AutoMapper;
using Domain.ValueObjects;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Location маппинг
        CreateMap<LocationDto, Location>();
        CreateMap<Location, LocationResponseDto>();
        
        // City маппинг
        CreateMap<CityDto, City>();
        CreateMap<City, CityResponseDto>();
    }
}