using TgBotGuide.Infrastructure.DataBase;
using TgBotGuide.Infrastructure.Migrator;

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new MissingConnectionStringException();
}

var factory = new TgBotGuideDbContextFactory();
var context = factory.CreateDbContext([connectionString]);

var migrationService = new MigrationService<TgBotGuideDbContext>(context);
migrationService.ApplyMigrations();