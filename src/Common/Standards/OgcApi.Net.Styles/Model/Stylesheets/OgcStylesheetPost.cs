namespace OgcApi.Net.Styles.Model.Stylesheets;

/// <summary>
/// A class used for adding new stylesheet to a storage
/// </summary>
public class OgcStylesheetPost : OgcStylesheetBase
{
    /// <summary>
    /// An identifier of the new style
    /// </summary>
    public required string StyleId { get; set; }

    /// <summary>
    /// A content of the stylesheet being added
    /// </summary>
    public required string Content { get; set; }
}