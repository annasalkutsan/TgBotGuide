using System.Linq.Expressions;
using AutoMapper;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Получение города по ID
        public async Task<CityResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var city = await _repository.GetByIdAsync(id);
            return _mapper.Map<CityResponseDto>(city);
        }

        // Получение всех городов
        public async Task<ICollection<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var cities = await _repository.GetAllAsync();
            return _mapper.Map<ICollection<CityResponseDto>>(cities);
        }

        // Поиск городов по условию
        public async Task<ICollection<CityResponseDto>> FindAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken)
        {
            var cities = await _repository.FindAsync(predicate);
            return _mapper.Map<ICollection<CityResponseDto>>(cities);
        }

        // Добавление нового города
        public async Task<CityResponseDto> AddAsync(CityDto dto, CancellationToken cancellationToken)
        {
            var city = _mapper.Map<City>(dto);
            await _repository.AddAsync(city);
            return _mapper.Map<CityResponseDto>(city);  // Возвращаем добавленный город
        }

        // Обновление города
        public async Task<CityResponseDto> UpdateAsync(Guid id, CityDto dto, CancellationToken cancellationToken)
        {
            var city = _mapper.Map<City>(dto);
            city.Id = id;
            await _repository.UpdateAsync(city);  // Используем асинхронный метод UpdateAsync
            return _mapper.Map<CityResponseDto>(city);  // Возвращаем обновленный город
        }

        // Удаление города
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var city = await _repository.GetByIdAsync(id);
            if (city != null)
            {
                await _repository.RemoveAsync(city);  // Используем асинхронный метод RemoveAsync
            }
        }

       
    }
}