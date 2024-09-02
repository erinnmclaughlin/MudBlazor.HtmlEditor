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

    [Parameter]
    public bool Outlined { get; set; } = true;

    [Parameter]
    public string Placeholder { get; set; } = "Tell your story...";

    [Parameter]
    public string Html { get; set; } = "";

    [Parameter]
    public EventCallback<string> HtmlChanged { get; set; }

    [Parameter]
    public Func<string, string, Stream, Task<string>>? ImageUploadHandler { get; set; }

    [Parameter]
    public string? ImageUploadUrl { get; set; }

    [Parameter]
    public IEnumerable<string> AllowedImageMimeTypes { get; set; } = new List<string>() { "image/png", "image/jpeg" };

    [Parameter]
    public bool Resizable { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    public async Task Reset()
    {
        await SetHtml(string.Empty);
    }

    public async Task SetHtml(string html)
    {
        if (_quill is not null)
            await _quill.InvokeVoidAsync("setHtml", html);

        if (HtmlChanged.HasDelegate)
            await HtmlChanged.InvokeAsync(html);

        Html = html;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);

            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.razor.js");

            var settings = new
            {
                Placeholder = Placeholder,
                BlazorImageUpload = ImageUploadHandler != null,
                ImageUploadUrl = ImageUploadUrl ?? "",
                AllowedImageMimeTypes = AllowedImageMimeTypes
            };

            _quill = await module.InvokeAsync<IJSObjectReference>("createQuillInterop", _dotNetRef, _editor, _toolbar, settings);

            await SetHtml(Html);

            StateHasChanged();
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
    public async Task<string> SaveImage(string imageName, string fileType, long size)
    {
        if(_quill is not null)
        {
            var imageJsStream = await _quill.InvokeAsync<IJSStreamReference>("getImageBytes", imageName);

            await using var imageStream = await imageJsStream.OpenReadStreamAsync(size);

            if (ImageUploadHandler is not null)
                return await ImageUploadHandler(imageName, fileType, imageStream);
        }

        return "";
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