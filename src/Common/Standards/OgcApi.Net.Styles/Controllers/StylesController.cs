using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OgcApi.Net.Resources;
using OgcApi.Net.Styles.Model;

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
        var styles = new OgcStyles();
        var nightStyle = new OgcStyle
        {
            Id = "night",
            Title = "Topographic night style",
            Links = [
                new Link {
                    Href = new Uri("https://example.com/api/v1/styles/night?f=mapbox"),
                    Type = "application/vnd.mapbox.style+json",
                    Rel = "stylesheet"
                }
            ]
        };
        styles.Styles.Add(nightStyle);

        return Ok(styles);
    }

    [HttpGet("{collectionId}/styles/{styleId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetStyle(string collectionId, string styleId, string? f)
    {
        if (string.IsNullOrEmpty(f))
        {
            // return style entry with links
        }

        // return stylesheet with format "f"
        return Ok();
    }

    [HttpGet("{collectionId}/styles/{styleId}/metadata")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetStyleMetadata(string collectionId, string styleId)
    {
        // return style metadata
        return Ok();
    }
}
