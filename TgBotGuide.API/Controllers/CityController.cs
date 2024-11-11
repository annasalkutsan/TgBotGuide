using Microsoft.AspNetCore.Mvc;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        // Получить все города
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _cityService.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        // Получить город по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _cityService.GetByIdAsync(id, cancellationToken);
            return Ok(response);
        }

        // Добавить город
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CityDto cityDto, CancellationToken cancellationToken)
        {
            var response = await _cityService.AddAsync(cityDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // Обновить город
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CityDto cityDto, CancellationToken cancellationToken)
        {
            var response = await _cityService.UpdateAsync(id, cityDto, cancellationToken);
            return Ok(response);
        }

        // Удалить город
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _cityService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}