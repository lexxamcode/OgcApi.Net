namespace OgcApi.Net.Styles.Model.Metadata;

/// <summary>
/// An interface representing style metadata storage
/// </summary>
public interface IMetadataStorage
{
    /// <summary>
    /// Returns metadata of the style
    /// </summary>
    /// <param name="collectionId">Base resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <returns>Style metadata</returns>
    public Task<OgcStyleMetadata> Get(string collectionId, string styleId);

    /// <summary>
    /// Replaces existing style metadata with provided metadata
    /// </summary>
    /// <param name="collectionId">Base resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <param name="newMetadata">New metadata</param>
    public Task Replace(string collectionId, string styleId, OgcStyleMetadata newMetadata);

    /// <summary>
    /// Updates existing metadata with provided metadata
    /// </summary>
    /// <param name="collectionId">Base resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <param name="updatedMetadata">Actual metadata</param>
    /// <returns>Updated metadata</returns>
    public Task<OgcStyleMetadata> Update(string collectionId, string styleId, OgcStyleMetadata updatedMetadata);
}
