using Microsoft.Extensions.DependencyInjection;

namespace Tizzani.MudBlazor.HtmlEditor.Services;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMudHtmlEditor(this IServiceCollection services)
    {
        return services.AddTransient<QuillJsInterop>();
    }
}
