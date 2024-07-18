using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VideoArchivesModels;
using VideosArchiveAPI.Helpers.Configs;
using VideosArchiveAPI.Interfaces;

namespace VideosArchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {        
        private readonly IWebHostEnvironment _env;
        private readonly UploadSettings _uploadSettings;
        private readonly IUploadsService _uploadsService;
        private readonly ILogger<VideosController> _logger;

        public VideosController(IWebHostEnvironment env, IUploadsService uploadsService, IOptions<UploadSettings> uploadSettings, ILogger<VideosController> logger)
        {
            _env = env;
            _uploadsService = uploadsService;
            _uploadSettings = uploadSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            return Ok(_uploadsService.GetVideos());
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormCollection form)
        {
            var files = form.Files;

            // Check if files were provided
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            // Check if the total size of uploaded files exceeds the limit
            if (Request.ContentLength > _uploadSettings.MaxFileSize)
            {
                return BadRequest("File size limit exceeded. Maximum allowed size is 200 MB.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "media");

            List<Video> newVideos = await _uploadsService.UploadVideos(files, uploadsFolder);

            return Ok(newVideos);
        }
    }
}
