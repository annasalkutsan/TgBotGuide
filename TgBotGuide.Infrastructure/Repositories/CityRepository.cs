using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Infrastructure.Repositories;

public class CityRepository(TgBotGuideDbContext context) : Repository<City>(context), ICityRepository;