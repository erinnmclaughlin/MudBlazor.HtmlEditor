using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor.Services;

public static class JSInterop
{
    public static Action<Guid>? OnTextChanged;

    [JSInvokable]
    public static void NotifyTextChanged(string editorId)
    {
        OnTextChanged?.Invoke(Guid.TryParse(editorId, out var id) ? id : Guid.Empty);
    }
}
