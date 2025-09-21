using OgcApi.Net.Styles.Model.Metadata;

namespace OgcApi.Net.Styles.Storage;

public interface IMetadataStorage
{
    public Task AddMetadata(string baseResource, string styleId, OgcStyleMetadata metadata);
}
