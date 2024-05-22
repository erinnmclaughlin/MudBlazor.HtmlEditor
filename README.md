<h1 align="center">Tizzani.MudBlazor.HtmlEditor</h1>

<p align="center">
  A customizable HTML editor component for <a href="https://mudblazor.com/">MudBlazor</a>, powered by <a href="https://quilljs.com/">QuillJS</a>.</p>
  
<p align="center">
  <img alt="NuGet Version" src="https://img.shields.io/nuget/v/Tizzani.MudBlazor.HtmlEditor">
  <img alt="NuGet Downloads" src="https://img.shields.io/nuget/dt/Tizzani.MudBlazor.HtmlEditor">
  <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/erinnmclaughlin/MudBlazor.HtmlEditor">
  <img alt="GitHub License" src="https://img.shields.io/github/license/erinnmclaughlin/MudBlazor.HtmlEditor">
</p>

<p align="center">
  <strong><small><a href="https://erinnmclaughlin.github.io/MudBlazor.HtmlEditor/">Try out the demo!</a></small></strong>
</p>

<br>

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/90f50c6b-b287-4e99-8849-added7239521)

Works in dark mode, too!

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/7755c8ac-fd95-4dab-8b5b-5360f04302b2)

## Installation

Download the [latest release](https://www.nuget.org/packages/Tizzani.MudBlazor.HtmlEditor) from NuGet:

```cmd
dotnet add package Tizzani.MudBlazor.HtmlEditor
```

Add references to the required CSS and JS to your main HTML file (e.g. `App.razor`, `index.html`, or `Page.cshtml` depending on your Blazor setup):

```html
<!-- Add to document <head> -->
<link href="_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.css" rel="stylesheet" />

<!-- Add to document <body> -->
<script src="https://cdn.jsdelivr.net/npm/quill@2.0.2/dist/quill.js"></script>
<script src="_content/Tizzani.MudBlazor.HtmlEditor/quill-blot-formatter.min.js"></script> <!-- optional; for image resize -->
```

Finally, add the following to your `_Imports.razor`:

```razor
@using Tizzani.MudBlazor.HtmlEditor
```

## Configuring Toolbar Options (available since v2.1)
There are several options available for customizing the HTML editor toolbar.

To customize options for a specific editor instance, define a `<MudHtmlToolbarOptions>` inside the `<MudHtmlEditor>`:

```razor
<MudHtmlEditor>
  <MudHtmlToolbarOptions InsertImage="false" /> <!-- This will exclude the "insert image" toolbar option -->
</MudHtmlEditor>
```

For all available options, see [here](./src/Tizzani.MudBlazor.HtmlEditor/MudHtmlToolbarOptions.razor.cs).

### Configuring Default Options
To configure default options for all instances of the HTML editor, you can wrap your razor content with `<CascadingMudHtmlToolbarOptions>`.

#### App.razor or Routes.razor
```razor
<CascadingMudHtmlToolbarOptions InsertImage="false">
  <Router AppAssembly="@typeof(Program).Assembly">
    <!-- etc. -->
  </Router>
</CascadingMudHtmlToolbarOptions>
```

Child components will inherit the default options, unless they override them with their own `<MudHtmlToolbarOptions>` instance.

### Advanced Customization
For more advanced customization, you can define your own toolbar options inside of an individual `<MudHtmlEditor>` component:

```razor
<MudHtmlEditor>
  <span class="ql-formats">
    <button class="ql-bold" type="button"></button>
    <button class="ql-italic" type="button"></button>
    <button class="ql-underline" type="button"></button>
    <button class="ql-strike" type="button"></button>
  </span>
</MudHtmlEditor>
```

See the [QuillJS documentation](https://quilljs.com/docs/modules/toolbar/) for more information on customizing the toolbar.

## Migrating from v1.0 to v2.0
* Remove the `services.AddMudBlazorHtmlEditor();` call from your `Startup.cs` or `Program.cs` file.
* Remove the `<script src="_content/Tizzani.MudBlazor.HtmlEditor/HtmlEditor.js">` tag from the document body. The required JS is now included by default.
