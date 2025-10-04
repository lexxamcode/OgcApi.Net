namespace OgcApi.Net.Styles.Storage;

public static class FormatToContentType
{
    private static readonly Dictionary<string, string> Mappings = new() {
        { "mapbox", "application/vnd.mapbox.style+json" },
        { "sld10", "application/vnd.ogc.sld+xml" },
        { "sld11", "application/vnd.ogc.sld+xml" }
    };

    public static string GetContentTypeForFormat(string format)
    {
        var isExtensionPresent = Mappings.TryGetValue(format, out var extension);
        if (!isExtensionPresent || extension is null)
            throw new Exception($"Not found file extension for file format {format}");

        return extension;
    }
}