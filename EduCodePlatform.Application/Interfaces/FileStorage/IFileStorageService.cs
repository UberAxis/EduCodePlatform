using Microsoft.AspNetCore.Http;

namespace EduCodePlatform.Application.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(IFormFile file);
        Task DeleteAsync(string path);
    }
}
