using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapGet("uploads/{fileName}", (string fileName) =>
{
    var path = Path.Combine("..", "uploads", fileName);
    var stream = new FileStream(path, FileMode.Open);
    return TypedResults.File(stream, "image/png");
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
