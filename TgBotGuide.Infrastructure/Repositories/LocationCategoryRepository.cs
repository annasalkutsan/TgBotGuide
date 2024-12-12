using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Infrastructure.Repositories;

public class LocationCategoryRepository(TgBotGuideDbContext context) : Repository<LocationCategory>(context), ILocationCategoryRepository;