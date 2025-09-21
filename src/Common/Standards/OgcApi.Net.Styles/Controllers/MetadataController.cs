using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OgcApi.Net.Styles.Controllers;

[EnableCors("OgcApi")]
[ApiController]
[Route("api/ogc/collections")]
[ApiExplorerSettings(GroupName = "ogc")]
public class MetadataController : ControllerBase
{
    [HttpGet("{collectionId}/styles/{styleId}/metadata")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetMetadata(string collectionId, string styleId)
    {
        // return style metadata
        return Ok();
    }

    [HttpPut("{collectionId}/styles/{styleId}/metadata")]
    public ActionResult ReplaceMetadata(string collectionId, string styleId)
    {
        return Ok();
    }

    [HttpPatch("{collectionId}/styles/{styleId}/metadata")]
    public ActionResult UpdateMetadata(string collectionId, string styleId)
    {
        return Ok();
    }
}
