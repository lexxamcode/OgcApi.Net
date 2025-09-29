using Microsoft.Extensions.Options;
using OgcApi.Net.Styles.Model.Metadata;
using System.Text.Json;

namespace OgcApi.Net.Styles.Storage.FileSystem;

public class StyleMetadataFileSystemStorage(IOptions<FileSystemStorageOptions> options) : IMetadataStorage
{
    private readonly FileSystemStorageOptions _options = options.Value;

    public async Task AddMetadata(string baseResource, string styleId, OgcStyleMetadata metadata)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, _options.MetadataFilename);
        var metadataContent = JsonSerializer.Serialize(metadata);
        await File.WriteAllTextAsync(metadataPath, metadataContent);
    }

    public async Task<OgcStyleMetadata> Get(string baseResource, string styleId)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, _options.MetadataFilename);
        var metadataContent = await File.ReadAllTextAsync(metadataPath);

        var metadata = JsonSerializer.Deserialize<OgcStyleMetadata>(metadataContent) ??
            throw new Exception("Failed to deserialize style metadata");
        return metadata;
    }

    
    public Task Replace(string baseResource, string styleId, OgcStyleMetadata newMetadata)
    {
        // In case of filesystem storage just override existing metadata file
        return AddMetadata(baseResource, styleId, newMetadata);
    }

    public Task Update(string baseResource, string styleId, OgcStyleMetadata updatedMetadata)
    {
        // In case of filesystem storage just override existing metadata file
        return AddMetadata(baseResource, styleId, updatedMetadata);
    }
}
