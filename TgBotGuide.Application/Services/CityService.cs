using System.Linq.Expressions;
using AutoMapper;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services;

public class CityService : ICityService
{
    private readonly IRepository<City> _repository;
    private readonly IMapper _mapper;

    public CityService(IRepository<City> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(id);
        return _mapper.Map<CityResponseDto>(city);
    }

    public async Task<ICollection<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cities = await _repository.GetAllAsync();
        return _mapper.Map<ICollection<CityResponseDto>>(cities);
    }

    public async Task<ICollection<CityResponseDto>> FindAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken)
    {
        var cities = await _repository.FindAsync(predicate);
        return _mapper.Map<ICollection<CityResponseDto>>(cities);
    }

    public async Task AddAsync(CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mapper.Map<City>(dto);
        await _repository.AddAsync(city);
    }

    public async Task UpdateAsync(Guid id, CityDto dto, CancellationToken cancellationToken)
    {
        var city = _mapper.Map<City>(dto);
        city.Id = id;
        _repository.Update(city);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(id);
        if (city != null)
        {
            _repository.Remove(city);
        }
    }
}