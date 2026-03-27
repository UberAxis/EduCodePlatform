using AutoMapper;
using EduCodePlatform.Application.DTOs.Modules;
using EduCodePlatform.Application.Interfaces.FileStorage.Modules;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModuleImageStorage _fileStorageService;

        public ModuleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IModuleImageStorage fileStorageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<GetModuleDTO>> GetAllAsync()
        {
            var modules = await _unitOfWork.Modules.GetAllAsync();
            return _mapper.Map<IEnumerable<GetModuleDTO>>(modules);
        }

        public async Task<GetModuleDTO> GetByIdAsync(int id)
        {
            var module = await _unitOfWork.Modules.GetByIdAsync(id);

            if (module == null)
                throw new KeyNotFoundException("Module not found");

            return _mapper.Map<GetModuleDTO>(module);
        }

        public async Task<GetModuleDTO> CreateAsync(CreateModuleDTO dto)
        {
            var title = dto.Title;

            var moduleExist = await _unitOfWork.Modules.ExistsByTitleAsync(title);

            if (moduleExist)
                throw new InvalidOperationException("Module with this name already exist");

            var imagePath = await _fileStorageService.SaveAsync(dto.CoverImage);

            var module = new Module(
                title: title,
                coverImage: imagePath,
                description: dto.Description,
                orderIndex: dto.OrderIndex
            );

            _unitOfWork.Modules.Add(module);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetModuleDTO>(module);
        }

        public async Task<GetModuleDTO> UpdateAsync(int id, UpdateModuleDTO dto)
        {
            var module = await _unitOfWork.Modules.GetByIdAsync(id);

            if (module == null)
                throw new KeyNotFoundException("Module not found");

            var title = dto.Title;

            var moduleExist = await _unitOfWork.Modules.ExistsByTitleAsync(title);

            if (moduleExist)
                throw new InvalidOperationException("Module with this name already exist");

            string? oldImagePath = module.CoverImage;
            string? newImage = null;

            var image = dto.CoverImage;

            if (image != null)
            {
                var imagePath = await _fileStorageService.SaveAsync(image);
                newImage = imagePath;

                if (!string.IsNullOrEmpty(oldImagePath))
                    await _fileStorageService.DeleteAsync(oldImagePath);
            }

            module.UpdateModule(
                title: title,
                coverImage: newImage,
                description: dto.Description,
                orderIndex: dto.OrderIndex
            );

            await _unitOfWork.SaveChangesAsync();

            if (image != null && oldImagePath != null)
            {
                await _fileStorageService.DeleteAsync(oldImagePath);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetModuleDTO>(module);
        }

        public async Task DeleteAsync(int id)
        {
            var module = await _unitOfWork.Modules.GetByIdAsync(id);

            if (module == null)
                throw new KeyNotFoundException("Module not found");

            var image = module.CoverImage;

            _unitOfWork.Modules.Delete(module);
            await _unitOfWork.SaveChangesAsync();

            if (image != null)
                await _fileStorageService.DeleteAsync(image);
        }
    }
}
