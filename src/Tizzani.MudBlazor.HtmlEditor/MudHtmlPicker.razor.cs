using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tizzani.MudBlazor.HtmlEditor
{
    public partial class MudHtmlPicker
    {
        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public string Name { get; set; } = default!;

        [Parameter]
        public Dictionary<string, string> Options { get; set; } = default!;

        [Parameter]
        public string Placeholder { get; set; } = "No placeholder set";

        private string GeneratedScript => $@"
            // Define a custom format
            const {Name.FirstCharToUpper()}Format = Quill.import('blots/inline');
            CustomFormat.blotName = '{Name}';
            CustomFormat.tagName = 'SPAN';
            Quill.register({Name.FirstCharToUpper()}Format, true);
        ";

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var listof = Options.ToList();

                var generatedCss = GenerateCss();
                await JS.InvokeVoidAsync("eval", $"document.head.insertAdjacentHTML('beforeend', `<style>{generatedCss}</style>`);");
                await JS.InvokeVoidAsync("eval", GeneratedScript);
            }
        }

        private string GenerateCss()
        {
            string generatedCss = $@"
                .ql-picker.ql-{Name} .ql-picker-label:before {{
                    content: '{Placeholder}';
                    width: 150px;
                }}
                ";

            foreach (var key in Options.Keys)
            {
                generatedCss += $@"
                .ql-picker.ql-{Name} .ql-picker-item[data-value='{key}']:before {{
                    content: '{Options[key]}';
                }}
            ";
            }

            return generatedCss;
        }
    }
}
