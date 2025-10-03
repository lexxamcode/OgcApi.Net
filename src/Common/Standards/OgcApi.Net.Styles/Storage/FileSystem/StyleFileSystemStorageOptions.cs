namespace OgcApi.Net.Styles.Storage.FileSystem;

/// <summary>
/// Style storage settings in the server file system
/// </summary>
public class StyleFileSystemStorageOptions
{
    /// <summary>
    /// Base directory where styles are stored
    /// </summary>
    public required string BaseDirectory { get; set; }

    /// <summary>
    /// A name of the file (with extension) where the default style is stored
    /// </summary>
    public required string DefaultStyleFilename { get; set; }

    /// <summary>
    /// A default name for a stylesheet (without extension)
    /// </summary>
    public required string StylesheetFilename { get; set; }

    /// <summary>
    /// A name of the file (with extension) where metadata of a style is stored
    /// </summary>
    public required string MetadataFilename { get; set; }
}
