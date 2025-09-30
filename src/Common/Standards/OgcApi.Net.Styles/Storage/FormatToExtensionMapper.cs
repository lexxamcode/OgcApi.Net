namespace OgcApi.Net.Styles.Storage;

public static class FormatToExtensionMapper
{
    private static readonly Dictionary<string, string> mappings = new() {
        { "mapbox", "json" },
        { "sld10", "xml" },
        { "sld11", "xml" }
    };

    public static string GetFileExtensionForFormat(string format)
    {
        var isExtensionPresent = mappings.TryGetValue(format, out var extension);
        if (!isExtensionPresent || extension is null)
            throw new Exception($"Not found file extension for file format {format}");

        return extension;
    }
}