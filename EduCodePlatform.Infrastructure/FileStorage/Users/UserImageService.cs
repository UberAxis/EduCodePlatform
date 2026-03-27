using EduCodePlatform.Application.Interfaces.FileStorage.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EduCodePlatform.Infrastructure.FileStorage.Users
{
    public class UserImageService : BaseFileStorageService ,IUserImageStorage
    {
        private readonly string _rootPath;
        private readonly string _requestPath;

        public UserImageService(
            IOptions<FileStorageOptions> options,
            IWebHostEnvironment env)
        {
            _requestPath = options.Value.RequestUserAvatarPath;

            _rootPath = Path.Combine(
                env.WebRootPath,
                "uploads",
                "users");

            FileDirectoryCheck(_rootPath);
        }

        public Task<string> SaveAsync(IFormFile file)
        {
            return BaseSaveAsync(file, _rootPath, _requestPath);
        }

        public Task DeleteAsync(string path)
        {
            return BaseDeleteAsync(path, _rootPath);
        }
    }
}
