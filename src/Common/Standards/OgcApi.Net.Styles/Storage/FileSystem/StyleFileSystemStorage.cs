using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OgcApi.Net.Resources;
using OgcApi.Net.Styles.Model.Metadata;
using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Model.Stylesheets;
using System.Text.Json;

namespace OgcApi.Net.Styles.Storage.FileSystem;

public class StyleFileSystemStorage(IOptionsMonitor<StyleFileSystemStorageOptions> options,
    IHttpContextAccessor httpContextAccessor) : IStyleStorage
{
    private readonly StyleFileSystemStorageOptions _options = options.CurrentValue;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Task<bool> StyleExists(string baseResource, string styleId)
    {
        var styleDirectory = Path.Combine(_options.BaseDirectory, baseResource, styleId);
        return Task.FromResult(Directory.Exists(styleDirectory));
    }

    public Task<bool> StylesheetExists(string baseResource, string styleId, string format)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(format);
        var stylesheetName = $"{_options.StylesheetFilename}.{format}.{stylesheetExtension}";
        var stylesheetPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, stylesheetName);

        return Task.FromResult(File.Exists(stylesheetPath));
    }

    public Task<List<string>> GetAvailableStylesheetsFormats(string baseResource, string styleId)
    {
        var stylesheetsPath = Path.Combine(_options.BaseDirectory, baseResource, styleId);

        if (!Directory.Exists(stylesheetsPath))
            return Task.FromResult(new List<string>());

        var stylesheets = Directory.GetFiles(stylesheetsPath);
        var availableFormats = stylesheets
            .Select(stylesheet => Path.GetFileName(stylesheet))
            .Where(stylesheet => stylesheet != _options.DefaultStyleFilename && stylesheet != _options.MetadataFilename)
            .Select(stylesheet =>
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
        var savePath = Path.Combine(_options.BaseDirectory, baseResource, parameters.StyleId);
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        await File.WriteAllTextAsync(Path.Combine(savePath, stylesheetName), parameters.Content);
    }

    public Task DeleteStyle(string baseResource, string styleId)
    {
        var stylePath = Path.Combine(_options.BaseDirectory, baseResource, styleId);

        if (!Directory.Exists(stylePath))
            return Task.CompletedTask;

        Directory.Delete(stylePath, true);
        return Task.CompletedTask;
    }

    public async Task<OgcStyle> GetStyle(string baseResource, string styleId)
    {
        var metadataPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, _options.MetadataFilename);
        if (!File.Exists(metadataPath))
            throw new KeyNotFoundException("Style metadata not found");

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
                    $"collections/{baseResource}/styles/{styleId}?f={format}"),
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

        if (!Directory.Exists(baseResourcePath))
            throw new KeyNotFoundException($"Styles for {baseResource} not found");

        var styles = new OgcStyles();
        var stylesDirectories = Directory.GetDirectories(baseResourcePath);
        foreach (var styleDirectory in stylesDirectories)
        {
            var styleId = Path.GetFileNameWithoutExtension(styleDirectory);
            var style = await GetStyle(baseResource, styleId);
            styles.Styles.Add(style);
        }

        var defaultStyleFilePath = Path.Combine(_options.BaseDirectory, baseResource, _options.DefaultStyleFilename);
        DefaultStyle? defaultStyle;
        if (!File.Exists(defaultStyleFilePath))
        {
            defaultStyle = new DefaultStyle
            {
                Default = styles.Styles.FirstOrDefault()?.Id
            };
        }
        else
        {
            var defaultStyleFileContent = await File.ReadAllTextAsync(defaultStyleFilePath);
            defaultStyle = JsonSerializer.Deserialize<DefaultStyle>(defaultStyleFileContent);
        }

        styles.Default = defaultStyle?.Default;
        return styles;
    }

    public async Task<string> GetStylesheet(string baseResource, string styleId, string format)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(format);
        var stylesheetFilename = $"{_options.StylesheetFilename}.{format}.{stylesheetExtension}";
        var stylesheetPath = Path.Combine(_options.BaseDirectory, baseResource, styleId, stylesheetFilename);
        if (!File.Exists(stylesheetPath))
            throw new KeyNotFoundException("Stylesheet not found");

        var content = await File.ReadAllTextAsync(stylesheetPath);
        return content;
    }

    public async Task ReplaceStyle(string baseResource, string styleId, StylesheetAddParameters stylePostParameters)
    {
        var stylesheetExtension = FormatToExtensionMapper.GetFileExtensionForFormat(stylePostParameters.Format);
        var stylesheetName = $"{_options.StylesheetFilename}.{stylePostParameters.Format}.{stylesheetExtension}";
        var path = Path.Combine(_options.BaseDirectory, baseResource, stylePostParameters.StyleId, stylesheetName);
        if (!File.Exists(path))
            throw new KeyNotFoundException("Stylesheet not found");

        await File.WriteAllTextAsync(path, stylePostParameters.Content);
    }

    public async Task UpdateDefaultStyle(string baseResource, DefaultStyle updateDefaultStyleRequest)
    {
        var defaultStyleFilePath = Path.Combine(_options.BaseDirectory, baseResource);
        if (!Directory.Exists(defaultStyleFilePath))
            Directory.CreateDirectory(defaultStyleFilePath);

        var defaultStyleFileContent = JsonSerializer.Serialize(updateDefaultStyleRequest);
        await File.WriteAllTextAsync(Path.Combine(defaultStyleFilePath, _options.DefaultStyleFilename), defaultStyleFileContent);
    }
}