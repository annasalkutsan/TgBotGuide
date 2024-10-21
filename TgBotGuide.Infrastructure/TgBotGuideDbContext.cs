using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure;

public class TgBotGuideDbContext: DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<LocationCategory> LocationsCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=12345;Database=your_database_name;");
        }
    }
}