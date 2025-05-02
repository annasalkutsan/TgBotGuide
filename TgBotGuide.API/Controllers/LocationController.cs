using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // Получить все локации
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _locationService.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        // Получить локацию по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _locationService.GetByIdAsync(id, cancellationToken);
            return Ok(response);
        }

        // Получить локацию по городу
        [HttpGet("by-city/{cityId}")]
        public async Task<IActionResult> GetByCity(Guid cityId, CancellationToken cancellationToken)
        {
            // Создаем предикат для поиска локаций по городу
            Expression<Func<Location, bool>> predicate = l => l.CityId == cityId;
        
            // Вызываем универсальный метод с предикатом
            var locations = await _locationService.FindAsync(predicate, cancellationToken);
        
            return Ok(locations);
        }
        
        // Получить локацию по городу
        [HttpGet("by-name/{locationName}")]
        public async Task<IActionResult> GetByName(string locationName, CancellationToken cancellationToken)
        {
            // Создаем предикат для поиска локаций по городу
            Expression<Func<Location, bool>> predicate = l => l.Name == locationName;
        
            // Вызываем универсальный метод с предикатом
            var locations = await _locationService.FindAsync(predicate, cancellationToken);
        
            return Ok(locations);
        }
        
        // Добавить локацию
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LocationDto locationDto, CancellationToken cancellationToken)
        {
            var response = await _locationService.AddAsync(locationDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // Обновить локацию
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LocationDto locationDto, CancellationToken cancellationToken)
        {
            var response = await _locationService.UpdateAsync(id, locationDto, cancellationToken);
            return Ok(response);
        }

        // Удалить локацию
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _locationService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}