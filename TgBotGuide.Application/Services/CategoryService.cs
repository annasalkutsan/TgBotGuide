using System.Linq.Expressions;
using AutoMapper;
using TgBotGuide.Application.Dto;
using TgBotGuide.Application.Dto.Response;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ILocationRepository _locationRepository; // Репозиторий для локаций
        private readonly ILocationCategoryRepository _locationCategoryRepository; // Репозиторий для связи
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository repository,
            ILocationRepository locationRepository,
            ILocationCategoryRepository locationCategoryRepository,
            IMapper mapper)
        {
            _repository = repository;
            _locationRepository = locationRepository;
            _locationCategoryRepository = locationCategoryRepository;
            _mapper = mapper;
        }

        // Получение категории по ID
        public async Task<CategoryResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(id);
            return _mapper.Map<CategoryResponseDto>(category);
        }

        // Получение всех категорий
        public async Task<ICollection<CategoryResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<ICollection<CategoryResponseDto>>(categories);
        }

        // Поиск категорий по условию
        public async Task<ICollection<CategoryResponseDto>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            var categories = await _repository.FindAsync(predicate);
            return _mapper.Map<ICollection<CategoryResponseDto>>(categories);
        }

        // Добавление новой категории
        public async Task<CategoryResponseDto> AddAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            return _mapper.Map<CategoryResponseDto>(category);
        }

        // Обновление категории
        public async Task<CategoryResponseDto> UpdateAsync(Guid id, CategoryDto dto, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(dto);
            category.Id = id;
            await _repository.UpdateAsync(category);  // Используем асинхронный метод UpdateAsync
            var updatedCategory = await _repository.GetByIdAsync(id); // Получаем обновленную категорию
            return _mapper.Map<CategoryResponseDto>(updatedCategory);
        }

        // Удаление категории
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category != null)
            {
                await _repository.RemoveAsync(category);  // Используем асинхронный метод RemoveAsync
            }
        }

        // Добавление локации к категории
        public async Task AddLocationToCategoryAsync(Guid categoryId, Guid locationId, CancellationToken cancellationToken)
        {
            // Получаем категорию и локацию по id
            var category = await _repository.GetByIdAsync(categoryId);
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (category == null || location == null)
            {
                throw new ArgumentException("Category or Location not found.");
            }

            // Создаем новую запись для связи между категорией и локацией
            var locationCategory = new LocationCategory
            {
                LocationId = locationId,
                CategoryId = categoryId
            };

            // Добавляем запись в репозиторий
            await _locationCategoryRepository.AddAsync(locationCategory);
        }
    }
}