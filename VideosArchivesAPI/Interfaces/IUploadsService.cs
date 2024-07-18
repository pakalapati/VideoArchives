using VideoArchivesModels;

namespace VideosArchiveAPI.Interfaces
{
    public interface IUploadsService
    {
        List<Video> GetVideos();
        Task<List<Video>> UploadVideos(IFormFileCollection files, string uploadsFolder);

    }
}
