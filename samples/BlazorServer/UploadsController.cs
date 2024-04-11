using Microsoft.AspNetCore.Mvc;

namespace BlazorServer;

public class UploadsController : Controller
{
    [HttpGet("uploads/{fileName}")]
    public IActionResult Get(string fileName)
    {
        var path = Path.Combine("C:\\temp", fileName);
        var stream = new FileStream(path, FileMode.Open);
        return File(stream, "image/png");
    }
}
