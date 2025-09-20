using OgcApi.Net.Resources;
using System.Text;
using System.Text.Json.Serialization;

namespace OgcApi.Net.Styles.Model;

/// <summary>
/// Stylesheet
/// </summary>
public class OgcStylesheet
{
    /// <summary>
    /// Title
    /// </summary>
    /// <remarks>
    /// The title of the encoding language,
    /// e.g. Mapbox Style, SLD, GeoCSS or CMSS
    /// </remarks>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    /// <remarks>
    /// The version of the encoding language
    /// used, e.g., 1.0
    /// </remarks>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Specification
    /// </summary>
    /// <remarks>
    /// A link to the style encoding specification,
    /// e.g. <see href="https://docs.mapbox.com/mapbox-gl-js/style-spec/"/> 
    /// </remarks>
    [JsonPropertyName("specification")]
    public string? Specification { get; set; }

    /// <summary>
    /// Native
    /// </summary>
    /// <remarks>
    /// True or False, indicating
    /// if this is the native encoding
    /// of the style, possibly hand prepared,
    /// or an on-the-fly conversion,
    /// with potential loss of details
    /// in the process
    /// </remarks>
    [JsonPropertyName("native")]
    public bool Native { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    [JsonPropertyName("link")]
    public required Link Link { get; set; }
}