using TgBotGuide.Domain.Interfaces;
using TgBotGuide.Infrastructure.DataBase.Repositories;

namespace TgBotGuide.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        return services;
    }
}