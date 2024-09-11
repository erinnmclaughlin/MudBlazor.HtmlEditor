using Microsoft.AspNetCore.Mvc;

namespace BlazorServer.Controllers
{
    [Route("[Controller]/[Action]")]
    public class FileController : Controller
    {
        private readonly IConfiguration _config;
        public FileController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var file = files[0];
            var root = _config.GetValue<string>(WebHostDefaults.ContentRootKey) ?? "";
            var path = Path.Combine(root, $"wwwroot{Path.DirectorySeparatorChar}images", file.FileName);
            FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(filestream);
            filestream.Close();

            return Content($"images/{file.FileName}");
        }
    }
}
