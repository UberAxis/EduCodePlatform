using Microsoft.AspNetCore.Http;

namespace EduCodePlatform.Infrastructure.FileStorage
{
    public class BaseFileStorageService
    {
        private const int MaxFileSize = 5 * 1024 * 1024;

        protected void FileDirectoryCheck(string rootPath)
        {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
        }

        protected void FileDeleteCheck(string fullPath)
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        protected void FileCheck(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            if (!file.ContentType.StartsWith("image/"))
                throw new ArgumentException("Only image files are allowed");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File too large");
        }

        protected async Task<string> BaseSaveAsync(IFormFile file, string rootPath, string requestPath)
        {
            FileCheck(file);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(rootPath, fileName);

            using var stream = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            await file.CopyToAsync(stream);

            return $"{requestPath}/{fileName}".Replace("\\", "/");
        }

        protected Task BaseDeleteAsync(string path, string rootPath)
        {
            var fullPath = Path.Combine(rootPath, Path.GetFileName(path));

            FileDeleteCheck(fullPath);

            return Task.CompletedTask;
        }
    }
}
