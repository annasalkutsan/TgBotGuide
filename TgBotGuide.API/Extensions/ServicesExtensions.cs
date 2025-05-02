using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Services;

namespace TgBotGuide.API.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ILocationService, LocationService>();
        return services;
    }
}