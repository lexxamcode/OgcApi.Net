using OgcApi.Net.Resources;
using System;
using System.Collections.Generic;

namespace OgcApi.Net.Modules;

public interface ILinksExtension
{
    public void AddLandingLinks(Uri baseUri, IList<Link> links);
    public List<Uri> GetConformsTo();
}
