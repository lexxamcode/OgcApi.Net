using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OgcApi.Net.Styles.Model;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Stylesheets;
using OgcApi.Net.Styles.Storage;

namespace OgcApi.Net.Styles.Controllers;

[EnableCors("OgcApi")]
[ApiController]
[Route("api/ogc/collections")]
[ApiExplorerSettings(GroupName = "ogc")]
public class StylesController(IStylesStorage stylesStorage, IMetadataStorage metadataStorage) : ControllerBase
{
    [HttpGet("{collectionId}/styles")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OgcStyles>> GetStyles(string collectionId)
    {
        try
        {
            var styles = await stylesStorage.GetStyles(collectionId);
            return Ok(styles);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpGet("{collectionId}/styles/{styleId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetStyle(string collectionId, string styleId, string? f)
    {
        try
        {
            if (string.IsNullOrEmpty(f))
            {
                var style = await stylesStorage.GetStyle(collectionId, styleId);
                return Ok(style);
            }

            var stylesheet = await stylesStorage.GetStylesheet(collectionId, styleId, f);
            return Ok(stylesheet);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{collectionId}/styles")]
    public async Task<ActionResult> PostStyle(string collectionId, [FromBody] OgcStylesheetPost addStyleParameters)
    {
        // Add new style
        var newStylesheet = await stylesStorage.AddStyle(collectionId, addStyleParameters);

        // Add a metadata for the new style
        var newlyAddedStyleMetadata = new OgcStyleMetadata
        {
            Id = addStyleParameters.StyleId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Stylesheets = [
                newStylesheet
            ]
        };
        await metadataStorage.AddMetadata(collectionId, addStyleParameters.StyleId, newlyAddedStyleMetadata);

        // return 201 Created
        return CreatedAtAction(
            nameof(GetStyle),
            new { collectionId, styleId = addStyleParameters.StyleId },
            null
        );
    }
}
