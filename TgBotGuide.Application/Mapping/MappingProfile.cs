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
        CreateMap<LocationDto, Location>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address(src.Street, src.House)))
            .ForMember(dest => dest.LocationsCategories, opt => opt.Ignore());

        CreateMap<Location, LocationResponseDto>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.House, opt => opt.MapFrom(src => src.Address.House))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.LocationsCategories.Select(lc => lc.Category)));

        // Category маппинг
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryResponseDto>();

        // City маппинг
        CreateMap<CityDto, City>();
        CreateMap<City, CityResponseDto>();
    }
}