using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IAsyncDisposable
{
    private IJSObjectReference? _module;

    private DotNetObjectReference<MudHtmlEditor>? _dotNetObjectReference;
    private ElementReference _editorReference;
    private ElementReference _toolbarReference;

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
    public bool Resizable { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    [Parameter]
    public Func<MudFileUploadEventArgs, Task<string>>? OnFileUploaded { get; set; }

    public async Task Reset()
    {
        await SetHtml(string.Empty);
    }

    public async Task SetHtml(string html)
    {
        if (_module is not null)
            await _module.InvokeVoidAsync("setHtml", html);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
            _module = null;
        }

        _dotNetObjectReference?.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetObjectReference = DotNetObjectReference.Create(this);

            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.razor.js");
            _module = await module.InvokeAsync<IJSObjectReference>("MudHtmlEditor.createInstance", _dotNetObjectReference, _editorReference, _toolbarReference, Placeholder, "snow");

            if (!string.IsNullOrWhiteSpace(Html))
                await SetHtml(Html);

            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task NotifyHtmlChanged(string html)
    {
        await HtmlChanged.InvokeAsync(html);
    }

    public async Task<string> HandleFileUpload(string fileName, IJSStreamReference streamRef)
    {
        if (_module is null || OnFileUploaded is null) return string.Empty;

        using var stream = await streamRef.OpenReadStreamAsync();

        var args = new MudFileUploadEventArgs(fileName, stream);

        try
        {
            return await OnFileUploaded(args);
        }
        catch { return string.Empty; }
    }
}

public record MudFileUploadEventArgs(string FileName, Stream Stream);