using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OgcApi.Net.Modules;
using OgcApi.Net.OpenApi.Interfaces;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Storage.FileSystem;

namespace OgcApi.Net.Styles.Extensions;

public static class StylesServicesExtensions
{
    public static IServiceCollection AddOgcApiStyles(this IServiceCollection services)
    {
        services.AddSingleton<ILinksExtension, StylesLinksExtension>();
        services.AddSingleton<IOpenApiExtension, StylesOpenApiExtension>();
        return services;
    }

    public static IServiceCollection AddStylesFileSystemStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StyleFileSystemStorageOptions>(configuration.GetSection(nameof(StyleFileSystemStorageOptions)));
        services.AddHttpContextAccessor();
        services.AddSingleton<IStyleStorage, StyleFileSystemStorage>();
        services.AddSingleton<IMetadataStorage, StyleMetadataFileSystemStorage>();

        return services;
    }
}