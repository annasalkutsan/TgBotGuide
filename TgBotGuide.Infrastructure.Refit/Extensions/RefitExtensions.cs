using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using TgBotGuide.Infrastructure.Refit.Interfaces;
using TgBotGuide.Infrastructure.Refit.Interfaces.Services;
using TgBotGuide.Infrastructure.Refit.Options;
using TgBotGuide.Infrastructure.Refit.Services;
using RefitSettings = TgBotGuide.Infrastructure.Refit.Options.RefitSettings;

namespace TgBotGuide.Infrastructure.Refit.Extensions;

public static class RefitExtensions
{
    public static IServiceCollection AddRefit(this IServiceCollection services, IConfiguration configuration)
    {
        // Зарегистрировать и валидировать RefitSettings
        services.AddOptions<RefitSettings>()
            .Bind(configuration.GetSection(nameof(RefitSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Refit-клиент с внедрением настроек через IServiceProvider
        services.AddRefitClient<ICityApi>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<RefitSettings>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
        services.AddScoped<ICityApiService, CityApiService>();

        services.AddRefitClient<ILocationApi>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<RefitSettings>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
        services.AddScoped<ILocationApiService, LocationApiService>();

        return services;
    }
}