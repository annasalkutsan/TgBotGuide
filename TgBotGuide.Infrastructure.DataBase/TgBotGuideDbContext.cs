using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.DataBase;

public class TgBotGuideDbContext: DbContext
{
    public TgBotGuideDbContext(DbContextOptions<TgBotGuideDbContext> options)
        : base(options)
    {
    }

    public TgBotGuideDbContext() { }

    public DbSet<City> Cities { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=12345;Database=TgBotDb;");
        }
    }
}