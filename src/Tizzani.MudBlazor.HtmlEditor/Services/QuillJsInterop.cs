using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor.Services;

public sealed class QuillJsInterop : IDisposable
{
    private readonly IJSRuntime _js;

    private QuillInstance? _quill;
    public QuillInstance Quill
    {
        get => _quill ?? throw new InvalidOperationException("Quill editor has not been initialized.");
        set
        {
            if (_quill != null)
                throw new InvalidOperationException("Quill editor has already been initialized.");

            _quill = value;
        }
    }

    public Action? OnTextChanged { get; set; }

    public QuillJsInterop(IJSRuntime js)
    {
        _js = js; 
        JSInterop.OnTextChanged += OnTextChangedInternal;
    }

    public async ValueTask InitializeAsync(QuillInstance quill, string? placeholder = null)
    {
        Quill = quill;
        await _js.InvokeVoidAsync("initializeQuill", quill.Id, quill.EditorReference, quill.ToolbarReference, placeholder);
    }

    public async Task<string> GetInnerHtmlAsync()
    {
        return await _js.InvokeAsync<string>("getQuillHtml", Quill.EditorReference);
    }

    public async ValueTask SetInnerHtmlAsync(string html)
    {
        await _js.InvokeVoidAsync("setQuillHtml", Quill.EditorReference, html);
    }

    public void Dispose()
    {
        JSInterop.OnTextChanged -= OnTextChangedInternal;
    }

    private void OnTextChangedInternal(Guid instanceId)
    {
        if (instanceId == Quill.Id)
            OnTextChanged?.Invoke();
    }
}