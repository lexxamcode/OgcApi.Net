using Microsoft.Extensions.Options;
using OgcApi.Net.Styles.Model.Metadata;
using System.Text.Json;

namespace OgcApi.Net.Styles.Storage.FileSystem;

public class StyleMetadataFileSystemStorage(IOptionsMonitor<StyleFileSystemStorageOptions> options) : IMetadataStorage
{
    private readonly StyleFileSystemStorageOptions _options = options.CurrentValue;

    public async Task Add(string baseResource, string styleId, OgcStyleMetadata metadata)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId);
        if (!Directory.Exists(metadataPath))
            Directory.CreateDirectory(metadataPath);

        var metadataContent = JsonSerializer.Serialize(metadata);
        await File.WriteAllTextAsync(Path.Combine(metadataPath, _options.MetadataFilename), metadataContent);
    }

    public async Task<OgcStyleMetadata> Get(string baseResource, string styleId)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId);
        if (!Directory.Exists(metadataPath))
            throw new KeyNotFoundException("Style not found");

        var metadataContent = await File.ReadAllTextAsync(Path.Combine(metadataPath, _options.MetadataFilename));

        var metadata = JsonSerializer.Deserialize<OgcStyleMetadata>(metadataContent) ??
            throw new Exception("Failed to deserialize style metadata");
        return metadata;
    }

    public Task Replace(string baseResource, string styleId, OgcStyleMetadata newMetadata)
    {
        // In case of filesystem storage just override existing metadata file
        return Add(baseResource, styleId, newMetadata);
    }

    public Task Update(string baseResource, string styleId, OgcStyleMetadata updatedMetadata)
    {
        // In case of filesystem storage just override existing metadata file
        return Add(baseResource, styleId, updatedMetadata);
    }
}