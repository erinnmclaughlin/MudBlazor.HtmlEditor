using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IAsyncDisposable
{
    private DotNetObjectReference<MudHtmlEditor>? _objRef;
    private IJSObjectReference? _quillRef;

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
        if (_quillRef != null)
            await _quillRef.InvokeVoidAsync("setHtml", html);
    }

    public async ValueTask DisposeAsync()
    {
        if (_quillRef != null)
            await _quillRef.DisposeAsync();

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
            _quillRef = await JS.InvokeAsync<IJSObjectReference>("createQuillInstance", _objRef, Editor, Toolbar, Placeholder);
            
            if (!string.IsNullOrWhiteSpace(Html))
                await SetHtml(Html);

            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task HandleFileUpload(string fileName, string mimeType, IJSStreamReference streamRef)
    {
        if (_quillRef is null || FileUploadHandler is null)
            return;

        using var stream = await streamRef.OpenReadStreamAsync();

        var url = await FileUploadHandler(this, new ImageUploadEventArgs
        {
            FileName = fileName,
            MimeType = mimeType,
            Stream = stream
        });

        await _quillRef.InvokeVoidAsync("insertImage", url);
    }

    [JSInvokable]
    public async Task NotifyHtmlChanged(string html)
    {
        if (Html != html)
            await HtmlChanged.InvokeAsync(html);
    }
}
