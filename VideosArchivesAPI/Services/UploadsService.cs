using VideoArchivesModels;
using VideosArchiveAPI.Interfaces;

namespace VideosArchiveAPI.Services
{
    public class UploadsService : IUploadsService
    {
        private static readonly List<Video> _videos = new();
        private readonly ILogger<UploadsService> _logger;

        public UploadsService(ILogger<UploadsService> logger) 
        {
            _logger = logger;
        }

        public List<Video> GetVideos()
        {
            return _videos;
        }

        public async Task<List<Video>> UploadVideos(IFormFileCollection files, string uploadsFolder)
        {
            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var newVideos = new List<Video>();

            // Process each uploaded file
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    // If file with the same name exists, overwrite it
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Add/update the video info in the list
                    var existingVideo = _videos.FirstOrDefault(v => v.FileName == file.FileName);

                    if (existingVideo != null)
                    {
                        existingVideo.FileSize = file.Length;
                    }
                    else
                    {
                        var relativePath = Path.Combine("media", file.FileName).Replace("\\", "/");

                        // add new videos to new videos list. This needs to be returned to UI so that we can append it to the table.
                        newVideos.Add(new Video
                        {
                            FileName = file.FileName,
                            FileSize = file.Length,
                            FilePath = relativePath
                        });

                        _logger.LogInformation($"File saved to {relativePath}");
                    }
                }
            }

            // if new videos count is greater than zero, add them to the list.
            if (newVideos.Count > 0)
            {
                _videos.AddRange(newVideos);
            }

            return newVideos;
        }
    }
}
