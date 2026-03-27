using EduCodePlatform.Application.Interfaces.FileStorage.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace EduCodePlatform.Infrastructure.FileStorage.Modules
{
    public class ModuleFileStorageService : BaseFileStorageService, IModuleImageStorage
    {
        private readonly string _rootPath;
        private readonly string _requestPath;

        public ModuleFileStorageService(
            IOptions<FileStorageOptions> options,
            IWebHostEnvironment env)
        {
            _requestPath = options.Value.RequestModulePath;

            _rootPath = Path.Combine(
                env.WebRootPath,
                "uploads",
                "modules");

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
