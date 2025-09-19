using OgcApi.Net.Resources;

namespace OgcApi.Net.Styles.Model;

public class OgcStyle
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<Link> Links { get; set; }
}
