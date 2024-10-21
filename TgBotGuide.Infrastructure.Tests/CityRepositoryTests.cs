using Microsoft.EntityFrameworkCore;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Infrastructure.Repositories;
using Xunit;

namespace TgBotGuide.Infrastructure.Tests
{
    public class CityRepositoryTests
    {
        private readonly TgBotGuideDbContext _context;
        private readonly Repository<City> _cityRepository;

        public CityRepositoryTests()
        {
            _context = new TgBotGuideDbContext();
            _cityRepository = new Repository<City>(_context);
        }

        [Fact]
        public async Task AddCity_ShouldAddCity()
        {
            // Arrange
            var city = new City {Name = "Test City2", Description = "Test Description2"};

            // Act
            await _cityRepository.AddAsync(city);
            await _context.SaveChangesAsync();

            // Assert
            var addedCity = await _cityRepository.GetByIdAsync(city.Id);
            Assert.NotNull(addedCity);
            Assert.Equal(city.Name, addedCity.Name);
        }

        [Fact]
        public async Task GetAllCities_ShouldReturnAllCities()
        {
            // Arrange
            var cities = new List<City>
            {
                new City {Name = "Test City3", Description = "Test Description3"},
                new City {Name = "Test City4", Description = "Test Description4"},
                new City {Name = "Test City5", Description = "Test Description5"}
            };

            await _cityRepository.AddRangeAsync(cities);

            // Act
            var result = await _cityRepository.GetAllAsync();

            // Assert
            Assert.Equal(cities.Count, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCity()
        {
            // Arrange
            var city = new City {Name = "Test City", Description = "Test Description"};
            await _cityRepository.AddAsync(city);

            // Act
            var retrievedCity = await _cityRepository.GetByIdAsync(city.Id);

            // Assert
            Assert.NotNull(retrievedCity);
            Assert.Equal(city.Name, retrievedCity.Name);
        }

        [Fact]
        public async Task UpdateCity_ShouldUpdateCity()
        {
            // Arrange
            var city = new City {Name = "Test City", Description = "Test Description"};
            await _cityRepository.AddAsync(city);

            // Act
            city.Name = "Updated Name";
            _cityRepository.Update(city);
            await _context.SaveChangesAsync();

            // Assert
            var updatedCity = await _cityRepository.GetByIdAsync(city.Id);
            Assert.Equal("Updated Name", updatedCity.Name);
        }

        [Fact]
        public async Task RemoveCity_ShouldDeleteCity()
        {
            // Arrange
            var city = new City {Name = "Test City", Description = "Test Description"};
            await _cityRepository.AddAsync(city);

            // Act
            _cityRepository.Remove(city);
            await _context.SaveChangesAsync();

            // Assert
            var deletedCity = await _cityRepository.GetByIdAsync(city.Id);
            Assert.Null(deletedCity);
        }
    }
}