using OgcApi.Net.Resources;
using System.Text.Json.Serialization;

namespace OgcApi.Net.Styles.Model;

/// <summary>
/// Style metadata
/// </summary>
public class OgcStyleMetadata
{
    /// <summary>
    /// Style Identifier
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    /// <remarks>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Keywords
    /// </summary>
    [JsonPropertyName("keywords")]
    public List<string>? Keywords { get; set; }

    /// <summary>
    /// Point of Contact
    /// </summary>
    [JsonPropertyName("pointOfContact")]
    public string? PointOfContant { get; set; }

    /// <summary>
    /// License
    /// </summary>
    [JsonPropertyName("license")]
    public string? License { get; set; }

    /// <summary>
    /// Created
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Updated
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Scope
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = "style";

    /// <summary>
    /// Version
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Stylesheets
    /// </summary>
    [JsonPropertyName("stylesheets")]
    public List<OgcStylesheet>? Stylesheets { get; set; }

    /// <summary>
    /// Data Layers
    /// </summary>
    [JsonPropertyName("layers")]
    public List<OgcLayer>? Layers { get; set; }

    /// <summary>
    /// Links
    /// </summary>
    [JsonPropertyName("links")]
    public List<Link>? Links { get; set; }
}