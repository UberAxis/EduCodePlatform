using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using EduCodePlatform.Application.Interfaces.FileStorage.Lessons;

namespace EduCodePlatform.Infrastructure.FileStorage.Lessons
{
    public class LessonCoverImageService : BaseFileStorageService, ILessonCoverImageStorage
    {
        private readonly string _rootPath;
        private readonly string _requestPath;

        public LessonCoverImageService(
            IOptions<FileStorageOptions> options,
            IWebHostEnvironment env)
        {
            _requestPath = options.Value.RequestLessonCoverPath;

            var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");

            _rootPath = Path.Combine(
                webRoot,
                "uploads",
                "lessons",
                "covers");

            FileDirectoryCheck(_rootPath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            return await BaseSaveAsync(file, _rootPath, _requestPath);
        }

        public Task DeleteAsync(string path)
        {
            return BaseDeleteAsync(path, _rootPath);
        }
    }
}
