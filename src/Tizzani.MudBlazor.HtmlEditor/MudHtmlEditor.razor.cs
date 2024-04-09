using Microsoft.AspNetCore.Components;
using Tizzani.MudBlazor.HtmlEditor.Services;

namespace Tizzani.MudBlazor.HtmlEditor;

public sealed partial class MudHtmlEditor : IDisposable
{
    private QuillInstance QuillInstance = new();

    [Inject]
    private QuillJsInterop Quill { get; set; } = default!;

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
        await Quill.SetInnerHtmlAsync("");
    }

    public void Dispose()
    {
        Quill.OnTextChanged -= UpdateInput;
        Quill.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Quill.InitializeAsync(QuillInstance, Placeholder);

            if (!string.IsNullOrWhiteSpace(Html))
                await Quill.SetInnerHtmlAsync(Html);

            Quill.OnTextChanged += UpdateInput;
            StateHasChanged();
        }
    }

    private async void UpdateInput()
    {
        var html = await Quill.GetInnerHtmlAsync();

        if (html != Html)
            await HtmlChanged.InvokeAsync(html);
    }
}