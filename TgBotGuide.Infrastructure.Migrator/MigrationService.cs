using Microsoft.EntityFrameworkCore;

namespace TgBotGuide.Infrastructure.Migrator;

/// <summary>
/// Сервис для применения миграций в контексте базы данных.
/// </summary>
/// <typeparam name="TContext">Тип контекста базы данных, производный от <see cref="DbContext"/>.</typeparam>
public class MigrationService<TContext> where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса миграций.
    /// </summary>
    /// <param name="context">Контекст базы данных, с которым будет работать сервис.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если параметр <paramref name="context"/> равен <c>null</c>.</exception>
    public MigrationService(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context), "Контекст не может быть null.");
    }

    /// <summary>
    /// Применяет все ожидающие миграции к базе данных.
    /// </summary>
    public void ApplyMigrations()
    {
        var pendingMigrations = _context.Database.GetPendingMigrations().ToList();

        if (pendingMigrations.Count == 0)
        {
            Console.WriteLine("Нет ожидающих миграций.");
            return;
        }

        Console.WriteLine($"Найдены следующие ожидающие миграции: {string.Join(", ", pendingMigrations)}");

        try
        {
            Console.WriteLine("Пытаемся применить миграции...");
            _context.Database.Migrate();
            Console.WriteLine("Миграции успешно применены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
            }
        }
    }
}