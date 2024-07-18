using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VideoArchives.Models;
using VideoArchivesModels;

namespace VideoArchives.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // call the api and return archives
            var videos = await _httpClient.GetFromJsonAsync<List<Video>>("https://localhost:7257/api/Videos");

            return View(videos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
