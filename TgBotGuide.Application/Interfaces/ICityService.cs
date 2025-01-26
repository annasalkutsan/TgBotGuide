using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Application.Interfaces;

public interface ICityService : ICrudService<City, CityDto, CityResponseDto>;