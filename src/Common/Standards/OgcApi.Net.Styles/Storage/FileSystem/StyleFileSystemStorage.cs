using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OgcApi.Net.Resources;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Model.Stylesheets;
using System.Text.Json;

namespace OgcApi.Net.Styles.Storage.FileSystem;

public class StyleFileSystemStorage(IOptions<StyleFileSystemStorageOptions> options,
    IHttpContextAccessor httpContextAccessor) : IStylesStorage
{
    private readonly StyleFileSystemStorageOptions _options = options.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Task<bool> StyleExists(string baseResource, string styleId)
    {
        var styleDirectory = Path.Combine(options.Value.BaseDirectory, baseResource, styleId);
        return Task.FromResult(Directory.Exists(styleDirectory));
    }

    public Task<bool> StylesheetExists(string baseResource, string styleId, string format)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(format);
        var stylesheetName = $"{_options.StylesheetFilename}.{format}.{stylesheetExtension}";
        var stylesheetPath = Path.Combine(options.Value.BaseDirectory, baseResource, styleId, stylesheetName);

        return Task.FromResult(File.Exists(stylesheetPath));
    }

    public Task<List<string>> GetAvailableStylesheetsFormats(string baseResource, string styleId)
    {
        var stylesheetsPath = Path.Combine(_options.BaseDirectory, baseResource, styleId);
        var stylesheets = Directory.GetFiles(stylesheetsPath);
        var availableFormats = stylesheets.Select(stylesheet =>
            Path.GetFileName(stylesheet)
            .Split(".")
            .Skip(1)
            .First())
        .ToList();

        return Task.FromResult(availableFormats);
    }

    public async Task AddStylesheet(string baseResource, StylesheetAddParameters parameters)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(parameters.Format);
        var stylesheetName = $"{_options.StylesheetFilename}.{parameters.Format}.{stylesheetExtension}";
        var savePath = Path.Combine(options.Value.BaseDirectory, baseResource, parameters.StyleId, stylesheetName);

        await File.WriteAllTextAsync(savePath, parameters.Content);
    }

    public Task DeleteStyle(string baseResource, string styleId)
    {
        var stylePath = Path.Combine(options.Value.BaseDirectory, baseResource, styleId);
        Directory.Delete(stylePath);
        return Task.CompletedTask;
    }

    public async Task<OgcStyle> GetStyle(string baseResource, string styleId)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, _options.MetadataFilename);
        var metadataContent = await File.ReadAllTextAsync(metadataPath);
        var metadata = JsonSerializer.Deserialize<OgcStyleMetadata>(metadataContent) ??
            throw new Exception("Style metadata does not exist");

        var links = new List<Link>();
        var availableFormats = await GetAvailableStylesheetsFormats(baseResource, styleId);
        foreach (var format in availableFormats)
        {
            var link = new Link
            {
                Href = new Uri(
                    Utils.GetBaseUrl(_httpContextAccessor.HttpContext?.Request),
                    $"collections/{baseResource}/style/{styleId}?f={format}"),
                Rel = "stylesheet",
                Type = FormatToContentType.GetContentTypeForFormat(format)
            };
            links.Add(link);
        }

        return new OgcStyle
        {
            Id = styleId,
            Title = metadata.Title,
            Links = links
        };
    }

    public async Task<OgcStyles> GetStyles(string baseResource)
    {
        var baseResourcePath = Path.Combine(_options.BaseDirectory, baseResource);

        var styles = new OgcStyles();
        var stylesDirectories = Directory.GetDirectories(baseResourcePath);
        foreach (var styleDirectory in stylesDirectories)
        {
            var styleId = Path.GetFileNameWithoutExtension(styleDirectory);
            var style = await GetStyle(baseResource, styleId);
            styles.Styles.Add(style);
        }

        var defaultStyleFilePath = Path.Combine(_options.BaseDirectory, baseResource, _options.DefaultStyleFilename);
        var defaultStyleFileContent = await File.ReadAllTextAsync(defaultStyleFilePath);
        var defaultStyle = JsonSerializer.Deserialize<DefaultStyle>(defaultStyleFileContent);

        styles.Default = defaultStyle?.Default;
        return styles;
    }

    public async Task<string> GetStylesheet(string baseResource, string styleId, string format)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(format);
        var stylesheetFilename = $"{_options.StylesheetFilename}.{format}.{stylesheetExtension}";
        var stylesheetPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, stylesheetFilename);

        var content = await File.ReadAllTextAsync(stylesheetPath);
        return content;
    }

    public async Task ReplaceStyle(string baseResource, string styleId, StylesheetAddParameters stylePostParameters)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(stylePostParameters.Format);
        var stylesheetName = $"{_options.StylesheetFilename}.{stylePostParameters.Format}.{stylesheetExtension}";
        var path = Path.Combine(options.Value.BaseDirectory, baseResource, stylePostParameters.StyleId, stylesheetName);

        await File.WriteAllTextAsync(path, stylePostParameters.Content);
    }

    public async Task UpdateDefaultStyle(string baseResource, DefaultStyle updateDefaultStyleRequest)
    {
        var defaultStyleFilePath = Path.Combine(_options.BaseDirectory, baseResource, _options.DefaultStyleFilename);
        var defaultStyleFileContent = JsonSerializer.Serialize(updateDefaultStyleRequest);
        await File.WriteAllTextAsync(defaultStyleFilePath, defaultStyleFileContent);
    }
}