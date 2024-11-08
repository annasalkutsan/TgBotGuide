using Domain.ValueObjects;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Infrastructure.Repositories;
using Xunit;

namespace TgBotGuide.Infrastructure.Tests
{
    public class LocationRepositoryTests
    {
        private readonly TgBotGuideDbContext _context;
        private readonly IRepository<Location> _locationRepository;

        public LocationRepositoryTests()
        {
            _context = new TgBotGuideDbContext();
            _locationRepository = new Repository<Location>(_context);
        }

        [Fact]
        public async Task AddLocation_ShouldAddLocation()
        {
            // Arrange
            var location = new Location
            (
                cityId: Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"),
                name: "Test Location",
                description: null, // Убедитесь, что Description nullable
                coordinates: "40.7128, -74.0060",
                address: new Address("Street", "123"),
                imageUrl: "https://example.com/image.jpg"
            );

            // Act
            await _locationRepository.AddAsync(location);
            await _context.SaveChangesAsync();

            // Assert
            var addedLocation = await _locationRepository.GetByIdAsync(location.Id);
            Assert.NotNull(addedLocation);
            Assert.Equal(location.Name, addedLocation.Name);
            Assert.Equal(location.Address.Street, addedLocation.Address.Street);
        }

        [Fact]
        public async Task GetAllLocations_ShouldReturnAllLocations()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location 1", "Desc 1", "40.7128, -74.0060", new Address("Street 1", "101"), "https://example.com/image1.jpg"),
                new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location 2", "Desc 2", "34.0522, -118.2437", new Address("Street 2", "102"), "https://example.com/image2.jpg"),
                new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location 3", "Desc 3", "51.5074, -0.1278", new Address("Street 3", "103"), "https://example.com/image3.jpg")
            };

            await _locationRepository.AddRangeAsync(locations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _locationRepository.GetAllAsync();

            // Assert
            Assert.Equal(locations.Count, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLocation()
        {
            // Arrange
            var location = new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location", "Test Description", "40.7128, -74.0060", new Address("Street", "123"), "https://example.com/image.jpg");
            await _locationRepository.AddAsync(location);
            await _context.SaveChangesAsync();

            // Act
            var retrievedLocation = await _locationRepository.GetByIdAsync(location.Id);

            // Assert
            Assert.NotNull(retrievedLocation);
            Assert.Equal(location.Name, retrievedLocation.Name);
            Assert.Equal(location.Address.Street, retrievedLocation.Address.Street);
            Assert.Equal(location.Description, retrievedLocation.Description);
        }

        [Fact]
        public async Task UpdateLocation_ShouldUpdateLocation()
        {
            // Arrange
            var location = new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location", "Original Description", "40.7128, -74.0060", new Address("Street", "123"), "https://example.com/image.jpg");
            await _locationRepository.AddAsync(location);
            await _context.SaveChangesAsync();

            // Act
            location.Name = "Updated Location";
            location.Address = new Address("Updated Street", "456");
            location.Description = "Updated Description";
            _locationRepository.Update(location);
            await _context.SaveChangesAsync();

            // Assert
            var updatedLocation = await _locationRepository.GetByIdAsync(location.Id);
            Assert.Equal("Updated Location", updatedLocation.Name);
            Assert.Equal("Updated Street", updatedLocation.Address.Street);
            Assert.Equal("Updated Description", updatedLocation.Description);
        }

        [Fact]
        public async Task RemoveLocation_ShouldDeleteLocation()
        {
            // Arrange
            var location = new Location(Guid.Parse("0192aeaa-c322-7491-a9d8-d9d06b022bfe"), "Test Location", "To Be Deleted", "40.7128, -74.0060", new Address("Street", "123"), "https://example.com/image.jpg");
            await _locationRepository.AddAsync(location);
            await _context.SaveChangesAsync();

            // Act
            _locationRepository.Remove(location);
            await _context.SaveChangesAsync();

            // Assert
            var deletedLocation = await _locationRepository.GetByIdAsync(location.Id);
            Assert.Null(deletedLocation);
        }
    }
}