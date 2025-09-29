using System.Text.Json.Serialization;

namespace OgcApi.Net.Styles.Model.Styles;

/// <summary>
/// Styles collection for a resource
/// </summary>
public class OgcStyles
{
    /// <summary>
    /// Default
    /// </summary>
    [JsonPropertyName("default")]
    public string? Default { get; set; }

    /// <summary>
    /// Styles list
    /// </summary>
    [JsonPropertyName("styles")]
    public List<OgcStyle> Styles { get; set; } = [];
}
