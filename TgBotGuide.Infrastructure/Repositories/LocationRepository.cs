using Shared.Infrastructure.Repositories;
using TgBotGuide.Application.Interfaces.Repositories;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.Repositories;

public class LocationRepository : Repository<Location>, ILocationRepository
{
    /// <summary>
    /// Конструктор репозитория для операций чтения с сущностями Skill
    /// </summary>
    /// <param name="dbContext">Контекст базы данных, необходимый для операций с сущностями</param>
    public LocationRepository(TgBotGuideDbContext dbContext) : base(dbContext)
    {
    }
}