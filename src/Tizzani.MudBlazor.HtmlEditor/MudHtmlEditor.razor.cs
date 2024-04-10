using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IDisposable
{
    private DotNetObjectReference<MudHtmlEditor>? _objRef;

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
    public bool Resizable { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    public async Task Reset()
    {
        await SetHtml(string.Empty);
    }

    public async Task SetHtml(string html)
    {
        Console.WriteLine("Setting html to " + html);
        await JS.InvokeVoidAsync("setQuillHtml", Editor, html);
    }

    public void Dispose()
    {
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
            await JS.InvokeVoidAsync("initializeQuill", _objRef, Editor, Toolbar, Placeholder);

            if (!string.IsNullOrWhiteSpace(Html))
                await SetHtml(Html);

            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task NotifyHtmlChanged(string html)
    {
        if (Html != html)
            await HtmlChanged.InvokeAsync(html);
    }
}