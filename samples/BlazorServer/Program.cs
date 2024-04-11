var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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
    return TypedResults.File(stream, "application/octet-stream");
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
