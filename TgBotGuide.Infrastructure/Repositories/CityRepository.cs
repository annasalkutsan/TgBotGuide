using Shared.Infrastructure.Repositories;
using TgBotGuide.Application.Interfaces.Repositories;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.Repositories;

public class CityRepository(TgBotGuideDbContext context) : Repository<City>(context), ICityRepository;