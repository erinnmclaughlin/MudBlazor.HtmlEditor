using Microsoft.AspNetCore.Components;

namespace Tizzani.MudBlazor.HtmlEditor;

/// <summary>
/// Defines the toolbar options that are available in the HtmlEditor.
/// </summary>
public partial class MudHtmlToolbarOptions
{
    [CascadingParameter]
    private MudHtmlToolbarOptions? DefaultOptions { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to select different headings and paragraph styles.
    /// </summary>
    [Parameter]
    public bool? TypographyPicker { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to bold text.
    /// </summary>
    [Parameter]
    public bool? Bold { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to italicize text.
    /// </summary>
    [Parameter]
    public bool? Italic { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to underline text.
    /// </summary>
    [Parameter]
    public bool? Underline { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to strike through text.
    /// </summary>
    [Parameter]
    public bool? Strike { get; set; }

    /// <summary>
    /// If <see langword="true"/>, a color picker for the foreground color will be available in the toolbar.
    /// </summary>
    [Parameter]
    public bool? ForegroundColorPicker { get; set; }

    /// <summary>
    /// If <see langword="true"/>, a color picker for the background color will be available in the toolbar.
    /// </summary>
    [Parameter]
    public bool? BackgroundColorPicker { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert subscripts and superscripts.
    /// </summary>
    [Parameter]
    public bool? SubSuperScript { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to align text (e.g., left, center, right, justify).
    /// </summary>
    [Parameter]
    public bool? Align { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert an unordered (numbered) list.
    /// </summary>
    [Parameter]
    public bool? OrderedList { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert an unordered (bulleted) list.
    /// </summary>
    [Parameter]
    public bool? UnorderedList { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of increasing/decreasing the indent size.
    /// </summary>
    [Parameter]
    public bool? Indent { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a hyperlink.
    /// </summary>
    [Parameter]
    public bool? InsertLink { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting an image.
    /// </summary>
    [Parameter]
    public bool? InsertImage { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a blockquote.
    /// </summary>
    [Parameter]
    public bool? Blockquote { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a code block.
    /// </summary>
    [Parameter]
    public bool? CodeBlock { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a horizontal rule.
    /// </summary>
    [Parameter]
    public bool? HorizontalRule { get; set; }

    private static bool Show(params bool?[] options) => options.Any(o => o is not false);

    protected override void OnInitialized()
    {
        if (DefaultOptions != null)
        {
            TypographyPicker ??= DefaultOptions.TypographyPicker;
            Bold ??= DefaultOptions.Bold;
            Italic ??= DefaultOptions.Italic;
            Underline ??= DefaultOptions.Underline;
            Strike ??= DefaultOptions.Strike;
            ForegroundColorPicker ??= DefaultOptions.ForegroundColorPicker;
            BackgroundColorPicker ??= DefaultOptions.BackgroundColorPicker;
            SubSuperScript ??= DefaultOptions.SubSuperScript;
            Align ??= DefaultOptions.Align;
            OrderedList ??= DefaultOptions.OrderedList;
            UnorderedList ??= DefaultOptions.UnorderedList;
            Indent ??= DefaultOptions.Indent;
            InsertLink ??= DefaultOptions.InsertLink;
            InsertImage ??= DefaultOptions.InsertImage;
            Blockquote ??= DefaultOptions.Blockquote;
            CodeBlock ??= DefaultOptions.CodeBlock;
            HorizontalRule ??= DefaultOptions.HorizontalRule;
        }
    }
}