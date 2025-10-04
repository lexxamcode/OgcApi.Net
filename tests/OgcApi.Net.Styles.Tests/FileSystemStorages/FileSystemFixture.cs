using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Storage.FileSystem;
using System.Text.Json;

namespace OgcApi.Net.Styles.Tests.FileSystemStorages;

public static class FileSystemFixture
{
    public const string CollectionId = "testCollection";
    public const string ExistingStyleId = "existingStyleId";

    public static void CreateInitialStyle(StyleFileSystemStorageOptions options)
    {
        const string testStyleContent = "style content";
        var testStylePath = Path.Combine(options.BaseDirectory, CollectionId, ExistingStyleId);

        // Create style
        Directory.CreateDirectory(testStylePath);
        File.WriteAllText(Path.Combine(testStylePath, "style.mapbox.json"), testStyleContent);

        // Create metadata
        var testStyleMetadata = new OgcStyleMetadata
        {
            Id = ExistingStyleId,
            Title = "ExistingStyleTitle",
            Description = "For test purpose only"
        };
        var testStyleMetadataContent = JsonSerializer.Serialize(testStyleMetadata);
        File.WriteAllText(Path.Combine(testStylePath, options.MetadataFilename), testStyleMetadataContent);

        // Create default style (null)
        File.WriteAllText(Path.Combine(options.BaseDirectory, CollectionId, options.DefaultStyleFilename), "{}");
    }
}
