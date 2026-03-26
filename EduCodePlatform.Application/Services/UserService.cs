using AutoMapper;
using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<GetUserDTO>>(users);
        }

        public async Task<GetUserDTO> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<GetUserDTO> CreateAsync(CreateUserDTO dto)
        {
            var name = dto.Name;

            var userExist = await _unitOfWork.Users.ExistsByNameAsync(name);

            if (userExist)
                throw new InvalidOperationException("User with this name already exist");

            var hash = _passwordHasher.Hash(dto.Password);

            var user = new User(
                name: name,
                hashPassword: hash
            );

            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<string> LoginAsync(LoginUserDTO dto)
        {
            var user = await _unitOfWork.Users.GetByNameAsync(dto.Name);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var hashCheck = _passwordHasher.Verify(user.HashPassword, dto.Password);

            if (!hashCheck)
                throw new UnauthorizedAccessException("Invalid email or password");

            return _tokenService.GenerateJSONWebToken(user);
        }

        public async Task<GetUserDTO> UpdateAsync(int id, UpdateUserDTO dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.UpdateUser(
                name: dto.Name
            );

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var hash = _passwordHasher.Hash(dto.Password);
                user.ChangePassword(hash);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
