using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using VideosArchiveAPI.Interfaces;
using VideosArchiveAPI.Services;

namespace VideosArchiveAPI.Test.Services
{
    public class UploadsServiceTests
    {
        private IUploadsService _uploadService;
        private Mock<ILogger<UploadsService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<UploadsService>>();
            _uploadService = new UploadsService(_mockLogger.Object);
        }

        private IFormFile CreateMockFiles(string name, string contentType, byte[] content)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(content);
            var writer = new StreamWriter(ms);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(name);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);

            return fileMock.Object;
        }

        [Test]
        public async Task UploadVideos_Should_Return_Successful_Count()
        {
            // Arrange
            var uploadPath = Path.GetTempPath();
            var files = new FormFileCollection
            {
                CreateMockFiles("test1.mp4", "video/mp4", new byte[100]),
                CreateMockFiles("test2.mp4", "video/mp4", new byte[200])
            };

            // Act
            var result = await _uploadService.UploadVideos(files, uploadPath);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
        }
    }
}
