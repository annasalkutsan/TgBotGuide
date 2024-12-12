using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;

namespace TgBotGuide.Infrastructure.Repositories;

public class CategoryRepository(TgBotGuideDbContext context) : Repository<Category>(context), ICategoryRepository;