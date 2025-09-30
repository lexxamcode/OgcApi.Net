using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OgcApi.Net.Styles.Model.Metadata;

namespace OgcApi.Net.Styles.Controllers;

[EnableCors("OgcApi")]
[ApiController]
[Route("api/ogc/collections")]
[ApiExplorerSettings(GroupName = "ogc")]
public class MetadataController(IMetadataStorage metadataStorage) : ControllerBase
{
    [HttpGet("{collectionId}/styles/{styleId}/metadata")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OgcStyleMetadata>> GetMetadata(string collectionId, string styleId)
    {
        try
        {
            var metadata = await metadataStorage.Get(collectionId, styleId);
            return Ok(metadata);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{collectionId}/styles/{styleId}/metadata")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ReplaceMetadata(string collectionId, string styleId, [FromBody] OgcStyleMetadata newMetadata)
    {
        try
        {
            await metadataStorage.Replace(collectionId, styleId, newMetadata);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPatch("{collectionId}/styles/{styleId}/metadata")]
    [Consumes("application/merge-patch+json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateMetadata(string collectionId, string styleId, [FromBody] OgcStyleMetadata metadata)
    {
        try
        {
            await metadataStorage.Update(collectionId, styleId, metadata);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}