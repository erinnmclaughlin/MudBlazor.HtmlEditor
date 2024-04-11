using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IAsyncDisposable
{
    private DotNetObjectReference<MudHtmlEditor>? _objRef;
    private IJSObjectReference? _module;

    public ElementReference Editor = default!;
    public ElementReference Toolbar = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Outlined { get; set; } = true;

    [Parameter]
    public string Placeholder { get; set; } = "Add a note...";

    [Parameter]
    public string Html { get; set; } = "";

    [Parameter]
    public EventCallback<string> HtmlChanged { get; set; }

    [Parameter]
    public Func<MudHtmlEditor, ImageUploadEventArgs, Task<string>>? FileUploadHandler { get; set; }

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
        if (_module != null)
            await _module.InvokeVoidAsync("setQuillHtml", html);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
            await _module.DisposeAsync();

        _objRef?.Dispose();
    }

    protected override void OnInitialized()
    {
        _objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.razor.js");
            _module = await module.InvokeAsync<IJSObjectReference>("createQuill", _objRef, Editor, Toolbar, Placeholder);
            
            if (!string.IsNullOrWhiteSpace(Html))
                await SetHtml(Html);

            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task<string> HandleFileUpload(string fileName, string mimeType, IJSStreamReference streamRef)
    {
        if (_module is null || FileUploadHandler is null)
            return string.Empty;

        using var stream = await streamRef.OpenReadStreamAsync();

        return await FileUploadHandler(this, new ImageUploadEventArgs
        {
            FileName = fileName,
            MimeType = mimeType,
            Stream = stream
        });
    }

    [JSInvokable]
    public async Task NotifyHtmlChanged(string html)
    {
        if (Html != html)
            await HtmlChanged.InvokeAsync(html);
    }
}
