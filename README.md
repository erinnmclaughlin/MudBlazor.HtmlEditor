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
In the `Page.cshtml` file (if Blazor Server) or `index.html` file (if Blazor WASM), add the following:
```html
<link href="_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.css" rel="stylesheet" />
```

```html
<script src="https://cdn.quilljs.com/1.3.6/quill.min.js"></script>
<script src="_content/Tizzani.MudBlazor.HtmlEditor/quill-blot-formatter.min.js"></script> <!-- optional; for image resize -->
<script src="_content/Tizzani.MudBlazor.HtmlEditor/MudHtmlEditor.js"></script>
```

In `Program.cs`, add the following
```cs
builder.Services.AddMudHtmlEditor();
```
