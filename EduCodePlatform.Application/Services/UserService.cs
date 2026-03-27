using AutoMapper;
using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<GetUserDTO>>(users);
        }

        public async Task<GetUserDTO> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<GetUserDTO> CreateAsync(CreateUserDTO dto)
        {
            var userExist = await _userManager.FindByNameAsync(dto.UserName);

            if (userExist != null)
                throw new InvalidOperationException("User with this name already exists");

            var user = new User(dto.UserName);

            var result = await _userManager.CreateAsync(user, dto.Password);

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
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid username or password");

            return _tokenService.GenerateJSONWebToken(user);
        }

        public async Task<GetUserDTO> UpdateProfileAsync(Guid userId, UpdateUserProfileDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                var existing = await _userManager.FindByNameAsync(dto.UserName);
                if (existing != null && existing.Id != userId)
                    throw new InvalidOperationException("UserName already taken");
                
                user.UserName = dto.UserName;
            }

            if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.AvatarUrl)) user.AvatarUrl = dto.AvatarUrl;
            if (dto.DateOfBirth.HasValue) user.DateOfBirth = dto.DateOfBirth;

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
                    throw new InvalidOperationException("Current password is required to change password");

                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!result.Succeeded)
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            user.UpdateUser(
                name: dto.Name
            );

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new KeyNotFoundException("User not found");

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
