namespace OgcApi.Net.Styles.Storage.FileSystem;

public class FileSystemStorageOptions
{
    public required string BaseDirectory { get; set; }
    public required string DefaultStyleFilename { get; set; }
    public required string StylesheetFilename { get; set; }
    public required string MetadataFilename { get; set; }
}
