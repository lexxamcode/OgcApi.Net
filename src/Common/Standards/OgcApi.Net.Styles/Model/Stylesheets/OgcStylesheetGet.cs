using OgcApi.Net.Resources;
using System.Text;
using System.Text.Json.Serialization;

namespace OgcApi.Net.Styles.Model.Stylesheets;

/// <summary>
/// Stylesheet with link to the file
/// </summary>
public class OgcStylesheetGet : OgcStylesheetBase
{
    /// <summary>
    /// Link
    /// </summary>
    [JsonPropertyName("link")]
    public required Link Link { get; set; }
}