using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OgcApi.Net.Styles.Model;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Model.Stylesheets;

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
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> PostStyle(string collectionId, [FromBody] StylesheetAddParameters addStyleParameters)
    {
        try
        {
            var existingStylesheet = await stylesStorage.GetStylesheet(collectionId,
                addStyleParameters.StyleId, addStyleParameters.Format);
            return Conflict();
        }
        catch(Exception)
        {
            // Add new stylesheet
            await stylesStorage.AddStylesheet(collectionId, addStyleParameters);

            // Add a metadata for the new style
            var newlyAddedStyleMetadata = new OgcStyleMetadata
            {
                Id = addStyleParameters.StyleId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };
            await metadataStorage.AddMetadata(collectionId, addStyleParameters.StyleId, newlyAddedStyleMetadata);

            // Return 201 Created
            return CreatedAtAction(
                nameof(GetStyle),
                new { collectionId, styleId = addStyleParameters.StyleId },
                null
            );
        }
    }

    [HttpPatch("{collectionId}/styles")]
    [Consumes("application/merge-patch+json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateDefaultStyle(string collectionId,
        [FromBody] DefaultStyle newDefaultStyle)
    {
        try
        {
            await stylesStorage.UpdateDefaultStyle(collectionId, newDefaultStyle);
            return Ok();
        }
        catch(KeyNotFoundException)
        {
            return NotFound();
        }
        catch(Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{collectionId}/styles/{styleId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ReplaceStyle(string collectionId, string styleId, [FromBody] StylesheetAddParameters stylesheetPost)
    {
        try
        {
            await stylesStorage.ReplaceStyle(collectionId, styleId, stylesheetPost);
            return Ok();
        }
        catch(KeyNotFoundException)
        {
            return NotFound();
        }
        catch(Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("{collectionId}/styles/{styleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteStyle(string collectionId, string styleId)
    {
        try
        {
            await stylesStorage.DeleteStyle(collectionId, styleId);
            return Ok();
        }
        catch(KeyNotFoundException)
        {
            return NotFound();
        }
        catch(Exception)
        {
            return StatusCode(500);
        }
    }
}
