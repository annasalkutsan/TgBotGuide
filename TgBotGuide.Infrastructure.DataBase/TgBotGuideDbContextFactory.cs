using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TgBotGuide.Infrastructure.DataBase;

/// <summary>
/// Фабрика для инициализации SkillDbContext
/// </summary>
public class TgBotGuideDbContextFactory : IDesignTimeDbContextFactory<TgBotGuideDbContext>
{
    public TgBotGuideDbContext CreateDbContext(string[] args)
    {
        Guard.Against.NullOrEmpty(args, nameof(args));
        var connectionString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<TgBotGuideDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new TgBotGuideDbContext(optionsBuilder.Options);
    }
}