using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using TgBotGuide.Infrastructure.Refit.Interfaces;
using TgBotGuide.Infrastructure.Refit.Interfaces.Services;
using TgBotGuide.Infrastructure.Refit.Options;
using TgBotGuide.Infrastructure.Refit.Services;

namespace TgBotGuide.Infrastructure.Refit.Extensions;

public static class RefitServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services, IConfiguration configuration)
    {
        // Зарегистрировать и валидировать RefitOptions
        services.AddOptions<RefitOptions>()
            .Bind(configuration.GetSection(nameof(RefitOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Refit-клиент с внедрением настроек через IServiceProvider
        services.AddRefitClient<ICityApi>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<RefitOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
        services.AddScoped<ICityApiService, CityApiService>();

        services.AddRefitClient<ILocationApi>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<RefitOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
        services.AddScoped<ILocationApiService, LocationApiService>();

        return services;
    }
}