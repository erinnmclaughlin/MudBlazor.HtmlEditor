# Tizzani.MudBlazor.HtmlEditor

A customizable HTML editor component for [MudBlazor](https://mudblazor.com/), powered by [QuillJS](https://quilljs.com/).

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/4c3e6b39-32b8-48bc-b25f-d10c93b0de30)

Works in dark mode, too!

![image](https://github.com/erinnmclaughlin/MudBlazor.HtmlEditor/assets/22223146/a1d24634-cee5-4d1e-8031-a7e1b842ad11)

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
