using Microsoft.AspNetCore.Mvc;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Interfaces;

namespace TgBotGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Получить все категории
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        // Получить категорию по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetByIdAsync(id, cancellationToken);
            return Ok(response);
        }

        // Добавить категорию
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var response = await _categoryService.AddAsync(categoryDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // Обновить категорию
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var response = await _categoryService.UpdateAsync(id, categoryDto, cancellationToken);
            return Ok(response);
        }

        // Удалить категорию
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        // Добавить локацию к категории
        [HttpPost("{categoryId}/locations/{locationId}")]
        public async Task<IActionResult> AddLocationToCategory(Guid categoryId, Guid locationId, CancellationToken cancellationToken)
        {
            await _categoryService.AddLocationToCategoryAsync(categoryId, locationId, cancellationToken);
            return NoContent();
        }
    }
}