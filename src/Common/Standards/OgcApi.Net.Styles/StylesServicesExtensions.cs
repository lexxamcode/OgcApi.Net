using Microsoft.Extensions.DependencyInjection;
using OgcApi.Net.Modules;

namespace OgcApi.Net.Styles;

public static class StylesServicesExtensions
{
    public static IServiceCollection AddOgcApiStylesLinks(this IServiceCollection services)
    {
        services.AddSingleton<ILinksExtension, StylesLinksExtension>();
        return services;
    }
}
