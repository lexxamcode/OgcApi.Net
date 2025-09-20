using OgcApi.Net.Resources;

namespace OgcApi.Net.Styles.Model;

/// <summary>
/// A layer involved in the symbolization
/// </summary>
public class OgcLayer
{
    /// <summary>
    /// Identifier
    /// </summary>
    /// <remarks>
    /// A layer id, typically the same
    /// identifier used in the style to
    /// refer to the layer
    /// </remarks>
    public required string Id { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Data Type
    /// </summary>
    /// <remarks>
    /// the type of data represented in the layer
    /// (vector, map, coverage, model)
    /// </remarks>
    public string? DataType { get; set; }

    /// <summary>
    /// Geometry Type
    /// </summary>
    /// <remarks>
    /// the geometry type of the features shown
    /// in this layer, if dataType is "vector"
    /// (points, lines, polygons, solids, any)
    /// </remarks>
    public string? GeometryType { get; set; }

    // PropertiesSchema ?

    /// <summary>
    /// Sample data link
    /// </summary>
    public Link? SampleData { get; set; }
}
