using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor.Services;

internal sealed class QuillJsInterop : IAsyncDisposable
{
    private readonly IJSObjectReference _quillInterop;

    public QuillJsInterop(IJSObjectReference quillInterop)
    {
        _quillInterop = quillInterop;
    }

    public async Task<string> GetInnerHtmlAsync()
    {
        return await _quillInterop.InvokeAsync<string>("getHtml");
    }

    public async ValueTask SetInnerHtmlAsync(string html)
    {
        await _quillInterop.InvokeVoidAsync("setHtml", html);
    }

    public async ValueTask DisposeAsync()
    {
        await _quillInterop.DisposeAsync();
    }
}