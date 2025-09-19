using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OgcApi.Net.Styles.Controllers;

[EnableCors("OgcApi")]
[ApiController]
[Route("api/ogc/collections")]
[ApiExplorerSettings(GroupName = "ogc")]
public class StylesController : ControllerBase
{
    [HttpGet("{collectionId}/styles")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetStyles(string collectionId)
    {
        return Ok();
    }
}
