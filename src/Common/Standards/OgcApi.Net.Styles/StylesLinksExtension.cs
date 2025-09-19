﻿using OgcApi.Net.Modules;
using OgcApi.Net.Resources;

namespace OgcApi.Net.Styles;

public class StylesLinksExtension : ILinksExtension
{
    public void AddLandingLinks(Uri baseUri, IList<Link> links)
    {
        links.Add(new Link
        {
            Href = new Uri(baseUri, "styles"),
            HrefLang = "en",
            Rel = "http://www.opengis.net/def/rel/ogc/1.0/styles",
            Type = "application/json",
            Title = "The styles shared via this API"
        });
    }

    public List<Uri> GetConformsTo()
    {
        return [
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/core"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/manage-styles"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/style-validation"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/resources"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/manage-resources"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/mapbox-styles"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/sld-10"),
            new Uri("http://www.opengis.net/spec/ogcapi-styles-1/1.0/conf/sld-11")
        ];
    }
}
