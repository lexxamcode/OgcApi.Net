﻿using OgcApi.Net.Styles.Model.Styles;
using OgcApi.Net.Styles.Model.Stylesheets;
using OgcApi.Net.Styles.Storage.FileSystem;
using OgcApi.Net.Styles.Tests.Mocks;

namespace OgcApi.Net.Styles.Tests.FileSystemStorages;

public class StyleFileSystemStorageTests : IDisposable
{
    private readonly StyleFileSystemStorageOptions _options;
    private readonly StyleFileSystemStorage _styleFileSystemStorage;

    public StyleFileSystemStorageTests()
    {
        var optionsMonitor = OptionsMonitorMock.Instance;
        var httpContextAccessor = HttpContextAccessorMock.Instance;
        _styleFileSystemStorage = new StyleFileSystemStorage(optionsMonitor, httpContextAccessor);
        _options = optionsMonitor.CurrentValue;

        FileSystemFixture.CreateInitialStyle(_options);
    }

    [Fact]
    public async Task StyleExists_ShouldReturnTrue_WhenStyleExists()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;

        var styleExists = await _styleFileSystemStorage.StyleExists(collectionId, styleId);
        Assert.True(styleExists);
    }

    [Fact]
    public async Task StyleExists_ShouldReturnFalse_WhenStyleDoesNotExist()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = "non-existing-style";

        var styleExists = await _styleFileSystemStorage.StyleExists(collectionId, styleId);
        Assert.False(styleExists);
    }

    [Fact]
    public async Task StylesheetExists_ShouldReturnTrue_WhenStylesheetExists()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string existingFormat = "mapbox";

        var styleExists = await _styleFileSystemStorage.StylesheetExists(collectionId, styleId, existingFormat);
        Assert.True(styleExists);
    }

    [Fact]
    public async Task StylesheetExists_ShouldReturnFalse_WhenStylesheetDoesNotExist()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string existingFormat = "sld10";

        var styleExists = await _styleFileSystemStorage.StylesheetExists(collectionId, styleId, existingFormat);
        Assert.False(styleExists);
    }

    [Fact]
    public async Task GetAvailableStylesheetsFormats_ShouldReturnAvailableFormats()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string expectedFormat = "mapbox";

        var availableFormats = await _styleFileSystemStorage.GetAvailableStylesheetsFormats(collectionId, styleId);

        Assert.NotNull(availableFormats);
        Assert.Single(availableFormats);
        Assert.Equal(expectedFormat, availableFormats.First());
    }

    [Fact]
    public async Task AddStylesheet_ShouldAddNewStyle()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string newStyleId = "newStyle";
        const string newStyleFormat = "sld10";
        const string newStyleContent = "completelyNewStyleContent";
        const string expectectedNewStylesheetName = "style.sld10.xml";

        var addParameters = new StylesheetAddParameters
        {
            StyleId = newStyleId,
            Format = newStyleFormat,
            Content = newStyleContent
        };
        await _styleFileSystemStorage.AddStylesheet(collectionId, addParameters);

        var path = Path.Combine(_options.BaseDirectory, collectionId, newStyleId, expectectedNewStylesheetName);
        var stylesheetExists = File.Exists(path);
        var content = File.ReadAllText(path);

        Assert.True(stylesheetExists);
        Assert.Equal(newStyleContent, content);
    }

    [Fact]
    public async Task AddStylesheet_ShouldAddNewStylesheetToExistingStyle()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string newStyleFormat = "sld10";
        const string newStyleContent = "NewStylesheetContent";
        const string expectectedNewStylesheetName = "style.sld10.xml";

        var addParameters = new StylesheetAddParameters
        {
            StyleId = styleId,
            Format = newStyleFormat,
            Content = newStyleContent
        };
        await _styleFileSystemStorage.AddStylesheet(collectionId, addParameters);
        var availableFormats = await _styleFileSystemStorage.GetAvailableStylesheetsFormats(collectionId, styleId);

        var path = Path.Combine(_options.BaseDirectory, collectionId, styleId, expectectedNewStylesheetName);
        var stylesheetExists = File.Exists(path);
        var content = File.ReadAllText(path);

        Assert.True(stylesheetExists);
        Assert.Equal(newStyleContent, content);
        Assert.Equal(2, availableFormats.Count);
        Assert.Equal("mapbox", availableFormats.First());
        Assert.Equal("sld10", availableFormats[1]);
    }

    [Fact]
    public async Task DeleteStyle_ShouldDeleteStyle()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        var stylePath = Path.Combine(_options.BaseDirectory, collectionId, styleId);

        await _styleFileSystemStorage.DeleteStyle(collectionId, styleId);
        Assert.False(Directory.Exists(stylePath));
    }

    [Fact]
    public async Task GetStyle_ShouldReturnStyleInfo()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string expectedTitle = "ExistingStyleTitle";
        const string expectedHref = "http://localhost/api/ogc/collections/testCollection/styles/existingStyleId?f=mapbox";

        var style = await _styleFileSystemStorage.GetStyle(collectionId, styleId);
        Assert.NotNull(style);
        Assert.Equal(expectedTitle, style.Title);
        Assert.Single(style.Links);
        Assert.Equal(expectedHref, style.Links.First().Href.ToString());
    }

    [Fact]
    public async Task GetStyles_ShouldReturnStyles()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        
        var styles = await _styleFileSystemStorage.GetStyles(collectionId);

        Assert.NotNull(styles);
        Assert.Single(styles.Styles);
        Assert.Null(styles.Default);
    }

    [Fact]
    public async Task GetStylesheet_ShouldGetStylesheetContent()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string format = "mapbox";
        const string expectedContent = "style content";

        var stylesheet = await _styleFileSystemStorage.GetStylesheet(collectionId, styleId, format);
        Assert.NotNull(stylesheet);
        Assert.Equal(expectedContent, stylesheet);
    }

    [Fact]
    public async Task ReplaceStyle_ShouldReplaceStylesheetContent()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;
        const string format = "mapbox";
        const string expectedContent = "Completely new content";

        var addStyleParameters = new StylesheetAddParameters
        {
            StyleId = styleId,
            Format = format,
            Content = expectedContent
        };
        await _styleFileSystemStorage.ReplaceStyle(collectionId, styleId, addStyleParameters);

        var stylePath = Path.Combine(_options.BaseDirectory, collectionId, styleId, "style.mapbox.json");
        var newContent = File.ReadAllText(stylePath);
        Assert.Equal(expectedContent, newContent);
    }

    [Fact]
    public async Task UpdateDefaultStyle_ShouldUpdateDefaultStyle()
    {
        const string collectionId = FileSystemFixture.CollectionId;
        const string styleId = FileSystemFixture.ExistingStyleId;

        var stylesInfo = await _styleFileSystemStorage.GetStyles(collectionId);
        Assert.Null(stylesInfo.Default);

        var defaultStyle = new DefaultStyle
        {
            Default = styleId
        };
        await _styleFileSystemStorage.UpdateDefaultStyle(collectionId, defaultStyle);

        stylesInfo = await _styleFileSystemStorage.GetStyles(collectionId);
        Assert.Equal(styleId, stylesInfo.Default);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Directory.Delete(_options.BaseDirectory, true);
        }
    }
}
