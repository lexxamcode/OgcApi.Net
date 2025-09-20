using OgcApi.Net.Styles.Model;

namespace OgcApi.Net.Styles.Storage;

public interface IMetadataStorage
{
    public Task Add(OgcStyleMetadata metadata);
    public Task Update(string styleId, OgcStyleMetadata updatedMetadata);
}