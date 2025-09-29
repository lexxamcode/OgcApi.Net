using System.Text.Json.Serialization;

namespace OgcApi.Net.Styles.Model.Styles;

/// <summary>
/// Class used for get/update default style for a baseResource
/// </summary>
public class DefaultStyle
{
    /// <summary>
    /// Default style identifier
    /// </summary>
    [JsonPropertyName("default")]
    public string? Default { get; set; }
}
