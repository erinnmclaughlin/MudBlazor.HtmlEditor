namespace Tizzani.MudBlazor.HtmlEditor;

public sealed class ImageUploadEventArgs
{
    public string FileName { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
    public Stream Stream { get; init; } = null!;
}
