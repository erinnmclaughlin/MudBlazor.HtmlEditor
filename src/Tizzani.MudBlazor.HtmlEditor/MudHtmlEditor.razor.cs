using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tizzani.MudBlazor.HtmlEditor.Services;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IAsyncDisposable
{
    private DotNetObjectReference<MudHtmlEditor>? _dotNetRef;
    private QuillJsInterop? _quill;
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
    public bool Resizable { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    public async Task Reset()
    {
        await SetHtml(string.Empty);
    }

    public async Task SetHtml(string html)
    {
        Console.WriteLine("Setting the html content");
        Console.WriteLine(html);

        if (_quill is not null)
        {
            await _quill.SetInnerHtmlAsync(html);
            StateHasChanged();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Console.WriteLine("first render!");

            _dotNetRef = DotNetObjectReference.Create(this);

            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.razor.js");
            var quill = await module.InvokeAsync<IJSObjectReference>("createQuillInterop", _dotNetRef, _editor, _toolbar, Placeholder);
            _quill = new QuillJsInterop(quill);

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