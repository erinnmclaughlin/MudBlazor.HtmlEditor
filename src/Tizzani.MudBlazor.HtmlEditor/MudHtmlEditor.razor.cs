using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IAsyncDisposable
{
    private DotNetObjectReference<MudHtmlEditor>? _dotNetRef;
    private IJSObjectReference? _quill;
    private ElementReference _toolbar;
    private ElementReference _editor;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Whether or not to ourline the editor. Default value is <see langword="true" />.
    /// </summary>
    [Parameter]
    public bool Outlined { get; set; } = true;

    /// <summary>
    /// The placeholder text to display when the editor has not content.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = "Tell your story...";

    /// <summary>
    /// The HTML markup from the editor.
    /// </summary>
    [Parameter]
    public string Html { get; set; } = "";

    /// <summary>
    /// Raised when the <see cref="Html"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> HtmlChanged { get; set; }

    /// <summary>
    /// The delta contents from the editor.
    /// </summary>
    [Parameter]
    public JsonObject Contents { get; set; } = new JsonObject();

    /// <summary>
    /// Raised when the <see cref="Contents"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<JsonObject> ContentsChanged { get; set; }

    /// <summary>
    /// The plain-text content from the editor.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = "";

    /// <summary>
    /// Raised when the <see cref="Text"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> TextChanged { get; set; }

    /// <summary>
    /// Whether or not the user can resize the editor. Default value is <see langword="true" />.
    /// </summary>
    [Parameter]
    public bool Resizable { get; set; } = true;

    /// <summary>
    /// Captures html attributes and applies them to the editor.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }


    /// <summary>
    /// Clears the content of the editor.
    /// </summary>
    public async Task Reset()
    {
        await SetHtml(string.Empty);
    }

    /// <summary>
    /// Sets the HTML content of the editor to the specified <paramref name="html"/>.
    /// </summary>
    public async Task SetHtml(string html)
    {
        if (_quill is not null)
            await _quill.InvokeVoidAsync("setHtml", html);

        HandleHtmlContentChanged(html);
        HandleTextContentChanged(await GetText());
        HandleContentsChanged(await GetContents());
    }

    /// <summary>
    /// Gets the current HTML content of the editor.
    /// </summary>
    public async Task<string> GetHtml()
    {
        if (_quill is not null)
            return await _quill.InvokeAsync<string>("getHtml");

        return "";
    }

    /// <summary>
    /// Gets the current delta contents of the editor.
    /// </summary>
    public async Task<JsonObject> GetContents()
    {
        if (_quill is not null)
            return await _quill.InvokeAsync<JsonObject>("getContents");

        return  new JsonObject();
    }

    /// <summary>
    /// Gets the current plain-text content of the editor.
    /// </summary>
    public async Task<string> GetText()
    {
        if (_quill is not null)
            return await _quill.InvokeAsync<string>("getText");

        return "";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);

            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.razor.js");
            try
            {
                _quill = await module.InvokeAsync<IJSObjectReference>("createQuillInterop", _dotNetRef, _editor, _toolbar, Placeholder);
                await SetHtml(Html);
                StateHasChanged();
            }
            catch (JSException)
            {
                // Creation failed due to missing DOM elements
            }
            catch (ObjectDisposedException)
            {
                // Component disposed before JS interop completed
            }
        }
    }

    [JSInvokable]
    public async void HandleHtmlContentChanged(string html)
    {
        if (Html == html) return; // nothing changed

        Html = html;
        await HtmlChanged.InvokeAsync(html);
    }

    [JSInvokable]
    public async void HandleTextContentChanged(string text)
    {
        if (Text == text) return; // nothing changed

        Text = text;
        await TextChanged.InvokeAsync(text);
    }

    [JSInvokable]
    public async void HandleContentsChanged(JsonObject contents)
    {
        if (JsonObject.Equals(Contents, contents)) return; // nothing changed

        Contents = contents;
        await ContentsChanged.InvokeAsync(contents);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_quill is not null)
        {
            await _quill.DisposeAsync();
            _quill = null;
        }

        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }
}