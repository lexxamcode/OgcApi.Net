using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OgcApi.Net.Modules;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Storage.FileSystem;

namespace OgcApi.Net.Styles.Extensions;

public static class StylesServicesExtensions
{
    public static IServiceCollection AddOgcApiStylesLinks(this IServiceCollection services)
    {
        services.AddSingleton<ILinksExtension, StylesLinksExtension>();
        return services;
    }
    
    public static IServiceCollection AddStylesFileSystemStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileSystemStorageOptions>(configuration.GetSection("FileSystemStorageOptions"));
        services.AddSingleton<IStylesStorage, StyleFileSystemStorage>();
        services.AddSingleton<IMetadataStorage, StyleMetadataFileSystemStorage>();

        return services;
    }
}
