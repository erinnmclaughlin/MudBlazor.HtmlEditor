using Microsoft.AspNetCore.Components;

namespace Tizzani.MudBlazor.HtmlEditor.Services;

public struct QuillInstance
{
    public Guid Id { get; }
    public ElementReference EditorReference;
    public ElementReference ToolbarReference;

    public QuillInstance()
    {
        Id = Guid.NewGuid();
    }
}
