namespace OgcApi.Net.Styles.Model.Stylesheets;

/// <summary>
/// A class used for adding new stylesheet to a storage
/// </summary>
public class StylesheetAddParameters
{
    /// <summary>
    /// An identifier of the new style
    /// </summary>
    public required string StyleId { get; set; }

    /// <summary>
    /// A format of the stylesheet being added, e.g. mapbox, sld10
    /// </summary>
    public required string Format { get; set; }

    /// <summary>
    /// A content of the stylesheet being added
    /// </summary>
    public required string Content { get; set; }
}