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

## Configuring Toolbar Options (available since v2.1)
There are several options available for customizing the HTML editor toolbar.

To customize options for a specific editor instance, define a `<MudHtmlToolbarOptions>` inside the `<MudHtmlEditor>`:

```html
<MudHtmlEditor>
  <MudHtmlToolbarOptions InsertImage="false" /> <!-- This will exclude the "insert image" toolbar option -->
</MudHtmlEditor>
```

For all available options, see [here](./src/Tizzani.MudBlazor.HtmlEditor/MudHtmlToolbarOptions.razor.cs).

### Configuring Default Options
To configure default options for all instances of the HTML editor, you can wrap your razor content with `<CascadingMudHtmlToolbarOptions>`.

#### App.razor or Routes.razor
```html
<CascadingMudHtmlToolbarOptions InsertImage="false">
  <Router AppAssembly="@typeof(Program).Assembly">
    <!-- etc. -->
  </Router>
</CascadingMudHtmlToolbarOptions>
```

Child components will inherit the default options, unless they override them with their own `<MudHtmlToolbarOptions>` instance.

### Advanced Customization
For more advanced customization, you can define your own toolbar options inside of an individual `<MudHtmlEditor>` component:

```html
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
