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
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool TypographyPicker { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to bold text.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Bold { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to italicize text.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Italic { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to underline text.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Underline { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to strike through text.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Strike { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, a color picker for the foreground color will be available in the toolbar.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool ForegroundColorPicker { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, a color picker for the background color will be available in the toolbar.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool BackgroundColorPicker { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert subscripts and superscripts.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool SubSuperScript { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to align text (e.g., left, center, right, justify).
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Align { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert an unordered (numbered) list.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool OrderedList { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option to insert an unordered (bulleted) list.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool UnorderedList { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of increasing/decreasing the indent size.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Indent { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a hyperlink.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool InsertLink { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting an image.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool InsertImage { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a blockquote.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Blockquote { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a code block.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool CodeBlock { get; set; } = true;

    /// <summary>
    /// If <see langword="true"/>, the toolbar will include the option of inserting a horizontal rule.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool HorizontalRule { get; set; } = true;

    protected override void OnInitialized()
    {
        if (DefaultOptions != null)
        {
            TypographyPicker = DefaultOptions.TypographyPicker;
            Bold = DefaultOptions.Bold;
            Italic = DefaultOptions.Italic;
            Underline = DefaultOptions.Underline;
            Strike = DefaultOptions.Strike;
            ForegroundColorPicker = DefaultOptions.ForegroundColorPicker;
            BackgroundColorPicker = DefaultOptions.BackgroundColorPicker;
            SubSuperScript = DefaultOptions.SubSuperScript;
            Align = DefaultOptions.Align;
            OrderedList = DefaultOptions.OrderedList;
            UnorderedList = DefaultOptions.UnorderedList;
            Indent = DefaultOptions.Indent;
            InsertLink = DefaultOptions.InsertLink;
            InsertImage = DefaultOptions.InsertImage;
            Blockquote = DefaultOptions.Blockquote;
            CodeBlock = DefaultOptions.CodeBlock;
            HorizontalRule = DefaultOptions.HorizontalRule;
        }
    }
}