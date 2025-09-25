using OgcApi.Net.Styles.Model;
using OgcApi.Net.Styles.Model.Stylesheets;

namespace OgcApi.Net.Styles.Model.Styles;

/// <summary>
/// An interface representing styles storage
/// </summary>
public interface IStylesStorage
{
    /// <summary>
    /// Gets a list of available styles for the resource
    /// </summary>
    /// <param name="baseResource">Resource identifier</param>
    /// <returns>OgcStyles object containing available styles</returns>
    public Task<OgcStyles> GetStyles(string baseResource);

    /// <summary>
    /// Get the style information including stylesheet links
    /// </summary>
    /// <param name="baseResource">Resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <returns>OgcStyle object representing style information</returns>
    public Task<OgcStyle> GetStyle(string baseResource, string styleId);

    /// <summary>
    /// Adds a new style to the styles storage
    /// </summary>
    /// <param name="baseResource">Base resource identifier</param>
    /// <param name="stylePostParameters">Style post parameters</param>
    /// <returns>Newly created stylesheet</returns>
    public Task<OgcStylesheetGet> AddStyle(string baseResource, OgcStylesheetPost stylePostParameters);

    /// <summary>
    /// Gets a stylesheet of the style with required format
    /// </summary>
    /// <param name="baseResource">Resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <param name="format">Required format</param>
    /// <returns>Stylesheet object with a link to the stylesheet file</returns>
    public Task<OgcStylesheetGet> GetStylesheet(string baseResource, string styleId, string format);

    /// <summary>
    /// Updates default style for baseResource
    /// </summary>
    /// <param name="baseResource">Base resource identifier</param>
    /// <param name="updateDefaultStyleRequest">Default style request</param>
    public Task UpdateDefaultStyle(string baseResource, UpdateDefaultStyleRequest updateDefaultStyleRequest);

    /// <summary>
    /// Replaces existing stylesheet with a new stylesheet
    /// </summary>
    /// <param name="baseResource">Base resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    /// <param name="stylePostParameters">Style post parameters</param>
    public Task ReplaceStyle(string baseResource, string styleId, OgcStylesheetPost stylePostParameters);

    /// <summary>
    /// Deletes existing style
    /// </summary>
    /// <param name="baseResource">Base resource identifier</param>
    /// <param name="styleId">Style identifier</param>
    public Task DeleteStyle(string baseResource, string styleId);
}
