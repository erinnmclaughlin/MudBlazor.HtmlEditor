# Tizzani.MudBlazor.HtmlEditor

A customizable HTML editor component for [MudBlazor](https://mudblazor.com/), powered by [QuillJS](https://quilljs.com/).

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/90f50c6b-b287-4e99-8849-added7239521)

Works in dark mode, too!

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/7755c8ac-fd95-4dab-8b5b-5360f04302b2)

### Demo
Try out the [demo](https://erinnmclaughlin.github.io/MudBlazor.HtmlEditor/) on GitHub Pages!

### Download from NuGet

```cmd
dotnet add package Tizzani.MudBlazor.HtmlEditor
```

### Setup
Add the following to your main HTML file (e.g. `App.razor`, `index.html`, or `Page.cshtml` depending on your Blazor setup):

```html
<link href="_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.css" rel="stylesheet" />
```

```html
<script src="https://cdn.jsdelivr.net/npm/quill@2.0.2/dist/quill.js"></script>
<script src="_content/Tizzani.MudBlazor.HtmlEditor/quill-blot-formatter.min.js"></script> <!-- optional; for image resize -->
```

Then add the following to your `_Imports.razor`:

```csharp
@using Tizzani.MudBlazor.HtmlEditor
```

## Migrating from v1.0 to v2.0
* Remove the `services.AddMudBlazorHtmlEditor();` call from your `Startup.cs` or `Program.cs` file.
* Remove the `<script src="_content/Tizzani.MudBlazor.HtmlEditor/HtmlEditor.js">` tag from the document body. The required JS is now included by default.
