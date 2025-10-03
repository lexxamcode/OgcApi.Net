using Microsoft.Extensions.Options;
using Moq;
using OgcApi.Net.Styles.Storage.FileSystem;

namespace OgcApi.Net.Styles.Tests.Mocks;

public static class OptionsMonitorMock
{
    public static IOptionsMonitor<StyleFileSystemStorageOptions> Instance
    {
        get
        {
            var options = new StyleFileSystemStorageOptions
            {
                BaseDirectory = "styles",
                StylesheetFilename = "style",
                MetadataFilename = "metadata.json",
                DefaultStyleFilename = "default.json",
            };
            var monitor = Mock.Of<IOptionsMonitor<StyleFileSystemStorageOptions>>(_ => _.CurrentValue == options);
            return monitor;
        }
    }
}
