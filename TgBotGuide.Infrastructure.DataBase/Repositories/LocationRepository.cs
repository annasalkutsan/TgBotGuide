using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Infrastructure.DataBase.Repositories;

public class LocationRepository(TgBotGuideDbContext context) : Repository<Location>(context), ILocationRepository;